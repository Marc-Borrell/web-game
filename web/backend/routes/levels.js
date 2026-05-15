const express = require('express');
const router = express.Router();
const pool = require('../db');

router.get('/', async (req, res) => {
    try {
        const result = await pool.query(
            'SELECT id, name FROM levels ORDER BY id ASC'
        );

        res.json(result.rows);
    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

router.get('/:id', async (req, res) => {
    const { id } = req.params;

    try {
        const result = await pool.query(
            'SELECT id, name FROM levels WHERE id = $1',
            [id]
        );

        if (!result.rows.length) {
            return res.status(404).json({ msg: "Nivel no encontrado" });
        }

        res.json(result.rows[0]);
    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

module.exports = router;