import React, {Component} from 'react';
import logo from '../images/Logo.PNG';
import {FaAlignJustify, FaUser, FaSuitcase, FaGripHorizontal, FaShoppingCart, FaClock, FaIdBadge, FaPen } from 'react-icons/fa';
import './NavBar.css';

import {Navbar, Dropdown, Nav, Button} from "react-bootstrap";
import {Link} from "react-router-dom";

class NavBar extends Component {

    constructor(props) {
        super(props);

        this.state = {
            loggedin: false,
            loggedinuser: {
                username: "",
                accessToken: "",
                refreshToken: ""
            }
        }
    }

    render() {
        return (
            <Navbar bg="light" id="navbar" sticky="top">

                <Nav>
                    <Dropdown className="mr-auto" drop="right">
                        <Dropdown.Toggle variant="light">
                            <FaAlignJustify className="icon dropdownbutton"/>
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <h3 align="center" className="usernameh2">Username</h3>
                            <Dropdown.Item className="dropdownitem"><FaSuitcase/>Post new lodging</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem"><FaGripHorizontal/>Manage existing lodging</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem" href="/about">About</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Nav>

                <Link  className="mx-auto" to="/">
                    <img src={logo} alt="logo" style={{height: 90, width: 260}}/>
                </Link>


                <Nav>
                    <Dropdown className="ml-auto" drop="left">
                        <Dropdown.Toggle variant="light">
                            <FaUser className="icon dropdownbutton"/>
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <h3 align="center" className="usernameh2">Username</h3>
                            <Dropdown.Item className="dropdownitem" disabled={!this.state.loggedin}> <FaShoppingCart/>Cart</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem" disabled={!this.state.loggedin}><FaClock/>Reservations</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem" disabled={!this.state.loggedin}><FaIdBadge/>Personal data</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem" disabled={!this.state.loggedin}><FaPen/>Ratings</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Link to="/login"><Button variant="outline-dark">Bejelentkezés</Button></Link>
                            <Dropdown.Divider/>
                            <Link to="/users"><Button variant="outline-dark">Felhasználók</Button></Link>
                        </Dropdown.Menu>
                    </Dropdown>
                </Nav>

            </Navbar>
        );
    }
}

export default NavBar;