const express = require('express');
const router = express.Router();
const pool = require('../db');

router.get('/', async (req, res) => {
    const { level_id } = req.query;

    if (!level_id) {
        return res.status(400).json({ msg: "No es troba l'entrada level_id a la base de dades" });
    }

    try {
        const ranking = await pool.query(
            `SELECT u.name, s.moves, s.time_ms
             FROM scores s
             JOIN users u ON u.id = s.user_id
             WHERE s.level_id = $1
             ORDER BY s.moves ASC, s.time_ms ASC
             LIMIT 50`,
            [level_id]
        );

        res.json(ranking.rows);

    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

module.exports = router;