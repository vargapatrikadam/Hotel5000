import React, { Component } from 'react';
import {Button, Form} from "react-bootstrap";
import './RegisterForm.css';

class RegisterForm extends Component {

    state={
        pass: null,
        confPass: null,
        btndisabled: true
    }

    comparePassword(event){
        event.preventDefault();

        if(this.pass.value === "" || this.confPass.value === ""){
            this.setState({
                btndisabled: true
            });
        }

        if(this.pass.value === this.confPass.value && this.pass.value !== ""){
            this.setState({
                btndisabled: false
            });
        }
    }

    render() {
        return (
            <div className="container">
                <Form>
                    <Form.Group controlId="formBasicEmail">
                        <Form.Label>Username</Form.Label>
                        <Form.Control type="text" placeholder="Enter username"/>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" ref={input => { this.pass = input }} onChange={e => this.comparePassword(e)} />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Confirm password</Form.Label>
                        <Form.Control type="password" placeholder="Confirm password" ref={input => { this.confPass = input }} onChange={e => this.comparePassword(e)} />
                    </Form.Group>
                    <Button variant="outline-dark" type="submit" block disabled={ this.state.btndisabled }>Sign up</Button>
                </Form>
            </div>
        );
    }
}

export default RegisterForm;