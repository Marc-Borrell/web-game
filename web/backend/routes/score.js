const express = require('express');
const router = express.Router();
const pool = require('../db');
const auth = require('../middleware/auth');

router.post('/', auth, async (req, res) => {
    const userId = req.user.id;
    const { level_id, moves, time_ms } = req.body;

    if (!level_id || !moves || !time_ms) {
        return res.status(400).json({ msg: "Dades incompletes" });
    }

    try {
        const existing = await pool.query(
            'SELECT * FROM scores WHERE user_id=$1 AND level_id=$2',
            [userId, level_id]
        );

        if (!existing.rows.length) {
            await pool.query(
                'INSERT INTO scores (user_id, level_id, moves, time_ms) VALUES ($1, $2, $3, $4)',
                [userId, level_id, moves, time_ms]
            );

            return res.json({ msg: "Score guardado" });
        }

        const old = existing.rows[0];

        const isBetter =
            moves > old.moves ||
            (moves === old.moves && time_ms < old.time_ms);

        if (isBetter) {
            await pool.query(
                'UPDATE scores SET moves=$1, time_ms=$2, created_at=NOW() WHERE user_id=$3 AND level_id=$4',
                [moves, time_ms, userId, level_id]
            );

            return res.json({ msg: "Score actualitzat" });
        }

        res.json({ msg: "Error: Score no millorat" });

    } catch (err) {
        console.error(err);
        res.status(500).json({ msg: "Error del servidor" });
    }
});

module.exports = router;