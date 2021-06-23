import React from 'react'
import {
  Container,
  Row,
  Col,
  Button
} from 'react-bootstrap'
import { authConfig } from '../services/AuthSettings'

const SignedIn = () => {
  return (
    <Container>
      <Row>
        <Col>
          <Button 
            href={`${authConfig.AUTH_URI}?client_id=${authConfig.CLIENT_ID}&logout_uri=${authConfig.LOGOUT_URI}`}>
              Logout
          </Button>
        </Col>
      </Row>
    </Container>
  )
}

export default SignedIn