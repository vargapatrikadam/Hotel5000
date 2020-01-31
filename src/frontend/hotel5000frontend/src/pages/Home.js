import React, {Component} from 'react';
import NavBar from "../components/NavBar";
import SearchBar from "../components/SearchBar";
import Footer from "../components/Footer";
import {Button} from "react-bootstrap";
import './Home.css';

class Home extends Component {
    render() {
        return (
            <div>
                <NavBar/>
                <SearchBar/>
                <div className="buttonContainer">
                    <Button variant="outline-dark" className="button">Browse lodgings</Button>
                    <Button variant="outline-dark" className="button">Register</Button>
                </div>
                <Footer/>
            </div>
        );
    }
}

export default Home;