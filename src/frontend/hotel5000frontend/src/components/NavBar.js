import React, {Component} from 'react';
import logo from '../images/Logo.PNG';
import {FaAlignJustify, FaUser, FaSuitcase, FaGripHorizontal, FaShoppingCart, FaClock, FaIdBadge, FaPen } from 'react-icons/fa';
import './NavBar.css';

import {Navbar, Dropdown, Nav} from "react-bootstrap";

class NavBar extends Component {

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
                        </Dropdown.Menu>
                    </Dropdown>
                </Nav>

                <a  className="mx-auto" href="/">
                    <img src={logo} alt="logo" style={{height: 90, width: 260}}></img>
                </a>


                <Nav>
                    <Dropdown className="ml-auto" drop="left">
                        <Dropdown.Toggle variant="light">
                            <FaUser className="icon dropdownbutton"/>
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <h3 align="center" className="usernameh2">Username</h3>
                            <Dropdown.Item className="dropdownitem"> <FaShoppingCart/>Cart</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem"><FaClock/>Reservations</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem"><FaIdBadge/>Personal data</Dropdown.Item>
                            <Dropdown.Divider/>
                            <Dropdown.Item className="dropdownitem"><FaPen/>Ratings</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Nav>

            </Navbar>
        );
    }
}

export default NavBar;