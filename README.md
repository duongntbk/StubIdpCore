This is a sample app for my blog post at the link below.

[https://duongnt.com/stub-idp](https://duongnt.com/stub-idp)

It is a port into .NET Core of *Sustainsys's* stub IDP which can be found [here](https://github.com/Sustainsys/Saml2/tree/master/Sustainsys.Saml2.StubIdp).

# Usage

## Build IDP from source code

Start the IDP with the following command.
```
dotnet run
```

The IDP will run on `https://localhost:5003` and `http://localhost:5002`

Use these following settings for the auto IDP. Your users will be logged in automatically after they are redirected to `/auto/login`.
```
"IdentityProviderIssuer": "http://localhost:5002/auto", // or "https://localhost:5003/auto"
"MetadataUrl": "http://localhost:5002/auto/metadata" // or "https://localhost:5003/auto/metadata"
```

And use these following settings for the interactive IDP. Your users can choose to input their credentials into `/interactive/login`.
```
"IdentityProviderIssuer": "http://localhost:5002/interactive", // or "https://localhost:5003/interactive"
"MetadataUrl": "http://localhost:5002/auto/interactive" // or "https://localhost:5003/interactive/metadata"
```

## Run IDP as a Docker container

Start the IDP as a Docker container by the following command.
```
docker run -dp 5002:80 duongntbk/stubidpcore:1.0
```

The IDP will run on `http://localhost:5002`

Use these following settings for the auto IDP. Your users will be logged in automatically after they are redirected to `/auto/login`.
```
"IdentityProviderIssuer": "http://localhost:5002/auto",
"MetadataUrl": "http://localhost:5002/auto/metadata"
```

And use these following settings for the interactive IDP. Your users can choose to input their credentials into `/interactive/login`.
```
"IdentityProviderIssuer": "http://localhost:5002/interactive",
"MetadataUrl": "http://localhost:5002/auto/interactive"
```

# License

MIT License

https://opensource.org/licenses/MIT
