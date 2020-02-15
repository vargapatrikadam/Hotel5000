import React, { Component } from 'react';
import {Button, Dropdown, Form} from "react-bootstrap";
import './RegisterForm.css';

class RegisterForm extends Component {

    constructor() {
        super();

        this.setSelectedRole = this.setSelectedRole.bind(this);

        this.state={
            username: "",
            email: "",
            firstName: "",
            lastName: "",
            password: "",

            role: "",
            fetchedData: []
            //btndisabled: true
        }
    }

    registerUrl = "https://localhost:5000/api/auth/regiser";

    /*comparePassword(event){
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
    }*/

    setSelectedRole(event){
        this.setState({
            role: event.target.innerText
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
            headers: {
                "Content-Type": "application/json",
                'Access-Control-Allow-Origin': '*',
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

                        <Dropdown className="mx-auto">
                            <Dropdown.Toggle variant="outline-dark" id="toggle">
                                {this.state.role}
                            </Dropdown.Toggle>

                            <Dropdown.Menu>
                                <Dropdown.Item onClick={this.setSelectedRole}>ApprovedUser</Dropdown.Item>
                                <Dropdown.Divider/>
                                <Dropdown.Item onClick={this.setSelectedRole}>Company</Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>


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