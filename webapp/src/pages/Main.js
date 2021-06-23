import React from 'react'
import {
  Container,
  Row,
  Col,
  Button
} from 'react-bootstrap'
import { authConfig } from '../services/AuthSettings'

const SignIn = () => {
  return (
    <Container>
      <Row>
        <Col>
          <Button 
            href={`${authConfig.AUTH_URI}?client_id=${authConfig.CLIENT_ID}&response_type=token&scope=${authConfig.AUTH_SCOPE}&redirect_uri=${authConfig.REDIRECT_URI}`}>
              Logout
          </Button>
        </Col>
      </Row>
    </Container>
  )
}

export default SignIn