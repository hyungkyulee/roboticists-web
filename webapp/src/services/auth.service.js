const API_URL = "https://u3fnac9d51.execute-api.eu-west-1.amazonaws.com/dev/auth/"

const register = async (email, password, firstname, lastname) => {
  const response = await fetch(API_URL + 'signup', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
    },
    body: {
      email: email,
      password: password,
      fullname: {firstname} + ' ' + {lastname}
    }
  })

  return await response.json()
}

const login = async (email, password) => {
  const response = await fetch(API_URL + 'signin', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
    },
    body: {
      email: email,
      password: password,
    }
  })

  const authResponse = await response.json()
  if(authResponse) {
    if(authResponse.data.accessToken()) {
      localStorage.setItem("user", JSON.stringify(authResponse.data))
    }
  }

  return authResponse.data
}

export {
  register, 
  login
}