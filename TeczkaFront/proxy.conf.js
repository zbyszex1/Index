const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:2492';

  // console.log(target);
  // console.log(env.ASPNETCORE_HTTPS_PORT);
  // console.log(env.ASPNETCORE_URLS);
  
const PROXY_CONFIG = [
  {
    context: [
      "/api/roles",
      "/api/classes",
      "/api/users",
      "/api/password",
      "/api/personal",
      "/api/articles",
      "/api/sections",
      "/api/persons",
      // "/api/persons/2",
      "/api/scans",
      "/api/indexes",
      "/api/weatherforecast",
      "/api/account",
      "/api/account/login",
      "/api/account/register",
      "/api/account/password",
      "/api/available",
      "/api/available/email",
      "/api/available/scans",
      "/api/available/sections",
      "/api/available/persons",
      "/api/available/test",
      "/api/classificate",
      "/api/generate",
      "/api/server",
      "/api/test",
      "/api/test/test",
      "/api/test/persons",
      "/api/test/scans",
      "/api/test/sections"
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
