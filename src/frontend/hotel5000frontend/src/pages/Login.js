import React, {Component} from 'react';
import Loginform from "../components/Loginform";
import NavBar from '../components/NavBar';

class Login extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <Loginform/>
            </div>
        );
    }
}

export default Login;