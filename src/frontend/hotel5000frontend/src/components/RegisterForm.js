import React, { Component } from 'react';
import {Button, Form} from "react-bootstrap";
import './RegisterForm.css';

class RegisterForm extends Component {

    constructor(props) {
        super(props);

        this.setSelectedRole = this.setSelectedRole.bind(this);

        this.state={
            username: "",
            email: "",
            firstName: "",
            lastName: "",
            password: "",

            role: "ApprovedUser",
            fetchedData: []
        }
    }



    setSelectedRole(event){
        this.setState({
            role: event.target.value
        })
        console.log(this.state.role)
    }

    handleUsername = (user) =>{
        this.setState({username: user.target.value})
    }
    handleFirstName = (first) =>{
        this.setState({firstName: first.target.value})
    }
    handleLastName = (last) =>{
        this.setState({lastName: last.target.value})
    }
    handleEmail = (mail) =>{
        this.setState({email: mail.target.value})
    }
    handlePassword = (pass) =>{
        this.setState({password: pass.target.value})
    }

    register = (username, password, email, firstName, lastName, role) =>{
        const data = {
            "username": username,
            "password": password,
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
            "role": role
        };

        console.log(this.state);
        fetch("https://localhost:5000/api/auth/register", {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data)
        }).catch(console.log) //TODO: kiegészíteni .then-el, ha a backend azon része kész ahol kapok vissza json bodyban egyedi status code-ot
    }


    render() {
        return (
            <div className="container">
                <Form>
                    <Form.Group>
                        <Form.Label>Username</Form.Label>
                        <Form.Control type="text" placeholder="Enter username" onChange={(user) => {this.handleUsername(user)}}/>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label>E-mail</Form.Label>
                        <Form.Control type="text" placeholder="Enter e-mail" onChange={(mail) => {this.handleEmail(mail)}}/>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label>First name</Form.Label>
                        <Form.Control type="text" placeholder="Enter your first name" onChange={(first) => {this.handleFirstName(first)}}/>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label>Last name</Form.Label>
                        <Form.Control type="text" placeholder="Enter your last name" onChange={(last) => {this.handleLastName(last)}}/>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label>Register as</Form.Label>

                        <Form.Control as="select" onChange={this.setSelectedRole}>
                            <option value="ApprovedUser">Individual</option>
                            <option value="Company">Company</option>
                        </Form.Control>
                    </Form.Group>



                    <Form.Group>
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" onChange={(pass) => {this.handlePassword(pass)}} />
                    </Form.Group>

                    <Button variant="outline-dark" type="button" block onClick={() => {this.register(this.state.username, this.state.password, this.state.email, this.state.firstName, this.state.lastName, this.state.role)}}>Sign up</Button>
                </Form>
            </div>
        );
    }
}

export default RegisterForm;