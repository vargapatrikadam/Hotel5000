import React, {Component} from 'react';
import SearchBar from "../components/SearchBar";
import {Button} from "react-bootstrap";
import './Home.css';
import {Link} from "react-router-dom";
import NavBar from '../components/NavBar';

class Home extends Component {
    render() {
        return (
            <div id="homeContainer">
                <NavBar/>
                <div id="searchContainer" className="mx-auto">
                    <SearchBar/>
                </div>
                <div id="buttonContainer" className="text-center">
                    <Link to="/lodgings">
                        <Button variant="outline-dark" style={{marginRight: 20}}>Browse lodgings</Button>
                    </Link>
                    <Link to="/register">
                        <Button variant="outline-dark" style={{marginLeft: 20}}>Register</Button>
                    </Link>
                </div>
            </div>
        );
    }
}

export default Home;