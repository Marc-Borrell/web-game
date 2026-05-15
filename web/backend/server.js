require('dotenv').config();
const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

const app = express();
const PORT = process.env.PORT || 4000;

app.use(cors());
app.use(bodyParser.json());

app.use('/users', require('./routes/users'));
app.use('/ranking', require('./routes/ranking'));
app.use('/score', require('./routes/score'));
app.use('/levels', require('./routes/levels'));

app.listen(PORT, () => {
    console.log(`Servidor connectat en el port ${PORT}`);
});