module.exports = {
  "/api": {
    target:
      process.env["services__BffGateway__https__0"] ||
      process.env["services__BffGateway__http__0"],
    secure: process.env["NODE_ENV"] !== "development"
  },
};
