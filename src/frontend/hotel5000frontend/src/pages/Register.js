import React, { Component } from 'react';
import RegisterForm from '../components/RegisterForm';
import NavBar from '../components/NavBar';

class Register extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <RegisterForm/>
            </div>
        );
    }
}

export default Register;