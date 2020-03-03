import React, {Component} from 'react';
import NavBar from '../components/NavBar';
import Lodging from "../components/Lodging";

class Lodgings extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <Lodging/>
            </div>
        );
    }
}

export default Lodgings;