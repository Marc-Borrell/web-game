const express = require('express');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const router = express.Router();
const pool = require('../db');
const { OAuth2Client } = require('google-auth-library');
const client = new OAuth2Client(process.env.GOOGLE_CLIENT_ID);

router.post('/register', async (req, res) => {
    const { name, email, password } = req.body;
    try {
        const exists = await pool.query('SELECT * FROM users WHERE email=$1', [email]);
        if (exists.rows.length) return res.status(400).json({ msg: "L'usuari ja existeix" });

        const hashedPassword = await bcrypt.hash(password, 10);

        const newUser = await pool.query(
            'INSERT INTO users (name, email, password) VALUES ($1, $2, $3) RETURNING id, name, email',
            [name, email, hashedPassword]
        );

        res.json({ msg: "Usuari registrat", user: newUser.rows[0] });
    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

router.post('/login', async (req, res) => {
    const { email, password } = req.body;

    try {
        const userResult = await pool.query(
            'SELECT * FROM users WHERE email=$1',
            [email]
        );

        if (!userResult.rows.length) {
            return res.status(400).json({ msg: "Usuari o contrasenya incorrecte" });
        }

        const user = userResult.rows[0];

        const match = await bcrypt.compare(password, user.password);

        if (!match) {
            return res.status(400).json({ msg: "Usuari o contrasenya incorrecte" });
        }

        const token = jwt.sign(
            { id: user.id, email: user.email },
            process.env.JWT_SECRET,
            { expiresIn: '7d' }
        );

        res.json({
            msg: "Login correcte",
            token,
            user: {
                id: user.id,
                name: user.name,
                email: user.email
            }
        });

    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});
    
const auth = require('../middleware/auth');

router.get('/me', auth, async (req, res) => {
    try {
        const userResult = await pool.query(
            'SELECT id, name, email FROM users WHERE id=$1',
            [req.user.id]
        );

        if (!userResult.rows.length) {
            return res.status(404).json({ msg: "Usuari no trobat" });
        }

        res.json(userResult.rows[0]);
    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

router.post('/google', async (req, res) => {
    const { googleToken } = req.body;
    try {
        
        const ticket = await client.verifyIdToken({
            idToken: googleToken,
            audience: process.env.GOOGLE_CLIENT_ID
        });
        const { email, name, sub: googleId } = ticket.getPayload();

        //Buscar o crear usuario
        let userResult = await pool.query('SELECT * FROM users WHERE email=$1', [email]);
        
        if (!userResult.rows.length) {
            userResult = await pool.query(
                'INSERT INTO users (name, email, password) VALUES ($1, $2, $3) RETURNING id, name, email',
                [name, email, googleId] // googleId como password placeholder
            );
        }

        const user = userResult.rows[0];
        const token = jwt.sign(
            { id: user.id, email: user.email },
            process.env.JWT_SECRET,
            { expiresIn: '7d' }
        );

        res.json({ msg: "Login correcte", token, user: { id: user.id, name: user.name, email: user.email } });
    } catch (err) {
        console.error(err);
        res.status(401).json({ msg: "Token de Google invàlid" });
    }
});

module.exports = router;