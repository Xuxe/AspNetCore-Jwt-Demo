# Demo Project for clarification of JsonWebTokens and parts of OAuth2

Simple .NET Core 2.x Project using JsonWebTokens with HMAC256 without a full stack OAuth2 Infrastructure. 

One Endpoint for Authorization and an other for Token Introspection for usage in a Microservice Infrastructure, without sharing the "secret" across services.

This project is absolutly only a demo project and provides some parts of the Token creation and validation with System.IdentityModel.Tokens.Jwt .
