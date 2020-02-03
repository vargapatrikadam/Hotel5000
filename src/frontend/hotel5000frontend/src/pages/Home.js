import React, {Component} from 'react';
import SearchBar from "../components/SearchBar";
import Footer from "../components/Footer";
import {Button} from "react-bootstrap";
import './Home.css';
import {Link} from "react-router-dom";

class Home extends Component {
    render() {
        return (
            <div>
                <SearchBar/>
                <div className="buttonContainer">
                    <Link to="/lodgings">
                        <Button variant="outline-dark" className="button">Browse lodgings</Button>
                    </Link>

                    <Button variant="outline-dark" className="button">Register</Button>
                </div>
                <Footer/>
            </div>
        );
    }
}

export default Home;