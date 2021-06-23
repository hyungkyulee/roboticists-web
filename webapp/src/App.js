import logo from './logo.svg';
import './App.css';
import {
  BrowserRouter as Router,
  Route,
  Switch,
} from "react-router-dom"
import { 
  Container,
  Row,
  Col,
  Button
} from 'react-bootstrap'
import { authConfig } from './services/AuthSettings'
import SignIn from './pages/SignIn'
import SignedIn from './pages/SignedIn'

function App() {

  return (
    <Router>
      <Container>
        <Row>
          <Col>
            <div>Roboticists Website - Header Section</div>
          </Col>
        </Row>
      </Container>

      <Switch>
        <Route path="/"><Main /></Route>
        <Route exact path="/signin"><SignIn /></Route>
        <Route exact path="/signedin"><SignedIn /></Route>
      </Switch>

      <Container>
        <Row>
          <Col>
            <div>Roboticists Website - Footer Section</div>
          </Col>
        </Row>
      </Container>
    </Router>
  );
}

export default App;
