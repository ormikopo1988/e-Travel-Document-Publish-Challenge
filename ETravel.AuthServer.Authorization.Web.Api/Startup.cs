﻿using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Net.Http.Headers;
using ETravel.AuthServer.Authorization.Web.Api.Providers;
using Microsoft.Owin.Security.OAuth;
using ETravel.AuthServer.Authorization.Web.Api.Formats;

[assembly: OwinStartup(typeof(ETravel.AuthServer.Authorization.Web.Api.Startup))]

namespace ETravel.AuthServer.Authorization.Web.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();
            
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
           
            ConfigureOAuth(app);

            WebApiConfig.Register(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

        }

        private void ConfigureOAuth(IAppBuilder app)
        {

            var  OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                //AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1500), //ONE DAY ACCESS TOKEN
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30), //ONE MONTH ACCESS TOKEN
                Provider = new CustomOAuthProvider(),
                // RefreshTokenProvider = new CustomRefreshTokenProvider()
                AccessTokenFormat = new JwtFormatter("8737e3f7a7984167b4d09f658a76bf32")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

        }
    }
}
