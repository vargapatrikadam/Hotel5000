import React, {Component} from 'react';
import Loginform from "../components/Loginform";
import NavBar from '../components/NavBar';

class Login extends Component {

    constructor(props){
        super(props)
        this.state =  {
            role: "",
            loggedin: false
        }
        this.getChildData = this.getChildData.bind(this)
    }

    componentDidUpdate(){
        console.log(this.state)
    }

    getChildData = (_role, _loggedin) => {
        this.setState({
            role: _role,
            loggedin: _loggedin
        })
    }

    render() {
        return (
            <div>
                <NavBar role={this.state.role} loggedin={this.state.loggedin}/>
                <Loginform parentData={this.getChildData}/>
            </div>
        );
    }
}

export default Login;