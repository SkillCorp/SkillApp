# SkillApp
SkillApp description

## Login
Login end pont is: `http://localhost:49940/api/jwt`

### Configuration
All routes are protected from unauthenticated access by default. 

If a developer likes to allow unauthenticated access he/she needs to do it explicitly with `[AllowAnonymous]` attribute.

If one likes to apply authorized access to specific endpoint, one can do it explicitly with `[Authorize(Policy = "Admin")]`. Check `SkillApp.WebApi.Startup` for new policies.

### Create a request (using **`cURL`**)
Login request will provide us with **`access_token`**.

```bash
curl --request POST --url http://localhost:49940/api/jwt --data UserName=Igor
```

Something like this should show up

```
{
    access_token":"your_access_token",
    "expires_in":300
}
```

Copy the `your_access_token` and use in the future requests.

```bash
curl -X GET -H "Authorization: Bearer your_access_token" "http://localhost:49940/api/values"
```