import React, {Component} from 'react';
import NavBar from "../components/NavBar";
import User from "../components/User";

class Users extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <User/>
            </div>
        );
    }
}

export default Users;