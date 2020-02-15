import React, {Component} from 'react';
import NavBar from '../components/NavBar';

class Error extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <h2>404 Page not found</h2>
            </div>
        );
    }
}

export default Error;