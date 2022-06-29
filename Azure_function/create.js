const sql = require("mssql");

module.exports = async function (context, req) {
  try {
    await sql.connect(process.env.CONNECTION_STRING);

    const {czas} = req.body;
    const result =
      await sql.query`insert into czas values(${czas})`;
    context.res = {
      status: 200,
      headers: {
        "Content-Type": "application/json",
      },
      message: "Time successfully added",
    };
  } catch (err) {
    context.res = {
      status: 400,
      headers: {
        "Content-Type": "application/json",
      },
      body: err,
    };
  }
};