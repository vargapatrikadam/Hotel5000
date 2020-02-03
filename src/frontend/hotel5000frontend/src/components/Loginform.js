import React, {Component} from 'react';
import {Button, Form} from "react-bootstrap";
import './Loginform.css';

class Loginform extends Component {
    render() {
        return (
            <div className="formContainer">
                <Form>
                    <Form.Group controlId="formBasicEmail">
                        <Form.Label>Username</Form.Label>
                        <Form.Control type="text" placeholder="Enter username"/>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password"/>
                    </Form.Group>
                    <Button variant="outline-dark" type="submit" block>Log In</Button>
                </Form>
            </div>
        );
    }
}

export default Loginform;