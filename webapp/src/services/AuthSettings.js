const authConfig = {
  AUTH_URI: "https://roboticists.auth.eu-west-1.amazoncognito.com/login",
  CLIENT_ID: "4g3chgr5ao7cv596h5tnsmgov",
  AUTH_SCOPE: "aws.cognito.signin.user.admin+email+openid+phone+profile",
  REDIRECT_URI: "http://localhost:3000/signedin",
  LOGOUT_URI: "http://localhost:3000"
}

export {
  authConfig,

}