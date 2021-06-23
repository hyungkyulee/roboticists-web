import React, {useState} from 'react'

import {
  Form,
  Button 
} from 'react-bootstrap';
import {register} from '../services/auth.service'

function SignUp() {
  const [form, setForm] = useState({
    email: '',
    password: '',
    firstname: '',
    lastname: ''
  })

  const handleSubmit = async () => {
    await register(form.email, form.password, form.firstname, form.lastname)
  }

  const handleOnChange = (e) => {
    console.log(`${e.target.name} : ${e.target.value}`)
    setForm([...e.target.name, e.target.value])
  }

  return (
    <Form>
      <Form.Group controlId="formEmail">
        <Form.Label>Email address</Form.Label>
        <Form.Control 
          type="email"
          name="email"
          placeholder="Enter email" 
          value={form.email}
          onChange={handleOnChange}
          />
        <Form.Text className="text-muted">
          We'll never share your email with anyone else.
        </Form.Text>
      </Form.Group>

      <Form.Group controlId="formPassword">
        <Form.Label>Password</Form.Label>
        <Form.Control 
          type="password" 
          name="password" 
          placeholder="Password" 
          value={form.password}
          onChange={handleOnChange}
          />
      </Form.Group>
      <Form.Group controlId="formSave">
        <Form.Check 
          type="checkbox" 
          name="save" 
          label="Check me out" 
          />
      </Form.Group>

      <Form.Group controlId="formFirstname">
        <Form.Label>First Name</Form.Label>
        <Form.Control 
          type="text" 
          name="firstname" 
          placeholder="First Name" 
          value={form.firstname}
          onChange={handleOnChange}
          />
      </Form.Group>
      <Form.Group controlId="formLastname">
        <Form.Label>Last Name</Form.Label>
        <Form.Control 
          type="text" 
          name="lastname" 
          placeholder="Last Name" 
          value={form.lastname}
          onChange={handleOnChange}
          />
      </Form.Group>

      <Button variant="primary" onClick={handleSubmit}>
        Submit
      </Button>
    </Form>
  );
}

export default SignUp;
