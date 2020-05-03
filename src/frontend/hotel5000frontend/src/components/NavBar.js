import React, {Component} from 'react';
import logo from '../images/Logo.PNG';
import {FaAlignJustify, FaUser, FaSuitcase, FaGripHorizontal, FaShoppingCart, FaClock, FaIdBadge, FaPen } from 'react-icons/fa';
import './NavBar.css';
import {Navbar, Dropdown, Nav, Button} from "react-bootstrap";
import {Link} from "react-router-dom";
import logout from "./LogoutHelper";

class NavBar extends Component {

    constructor(props) {
        super(props);

        this.state = {
            loggedin: false,
            role: '',
            username: ""
        }
    }

    getDataFromLocal = () => {
        let obj = {
            role: sessionStorage.getItem('role'),
            loggedin: sessionStorage.getItem('loggedin'),
            username: sessionStorage.getItem('username')
        }
        this.setState({loggedin: obj.loggedin, role: obj.role, username: obj.username})
    }

    componentDidMount(){
        this.getDataFromLocal()
    }

    logOut = () => {
        logout().then(
            alert("Successfully logged out.")
        )
    }

    render() {
        return (
            <div onMouseEnter={this.getDataFromLocal}>
                <Navbar bg="light" id="navbar" sticky="top">

                    <Nav>
                        <Dropdown className="mr-auto" drop="right">
                            <Dropdown.Toggle variant="light">
                                <FaAlignJustify className="icon dropdownbutton"/>
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                <h3 align="center" className="usernameh2" hidden={!this.state.loggedin}>{this.state.username}</h3>
                                <Link to="/postLodging"><Button variant="outline-dark" style={{width: '15rem'}}><FaSuitcase/>Post new lodging</Button></Link>
                                <Dropdown.Divider/>
                                <Link to="/ownReservations"><Button variant="outline-dark" style={{width: '15rem'}}><FaGripHorizontal/>Own reservations</Button></Link>
                                <Dropdown.Divider/>
                                <Link to="/ownLodgings">
                                    <Button variant="outline-dark" style={{width: '15rem'}}>
                                        <FaGripHorizontal/>Own lodgings
                                    </Button>
                                </Link>
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
                                <h3 align="center" className="usernameh2" hidden={!this.state.loggedin}>{this.state.username}</h3>
                                <Dropdown.Item className="dropdownitem" disabled={this.state.loggedin !== true}> <FaShoppingCart/>Cart</Dropdown.Item>
                                <Dropdown.Divider/>
                                <Dropdown.Item className="dropdownitem" disabled={this.state.loggedin !== true}><FaClock/>Reservations</Dropdown.Item>
                                <Dropdown.Divider/>
                                <Dropdown.Item className="dropdownitem" disabled={this.state.loggedin !== true}><FaIdBadge/>Personal data</Dropdown.Item>
                                <Dropdown.Divider/>
                                <Dropdown.Item className="dropdownitem" disabled={this.state.loggedin !== true}><FaPen/>Ratings</Dropdown.Item>
                                <Dropdown.Divider/>
                                <Link to="/login"><Button variant="outline-dark" style={{width: '10rem'}}>Sign in</Button></Link>
                                <Dropdown.Divider/>
                                <Link to="/register">
                                    <Button variant="outline-dark" style={{width: "10rem"}}>Register</Button>
                                </Link>
                                <Dropdown.Divider/>
                                <Button variant="outline-dark" style={{width: "10rem"}}
                                        onClick={() => {this.logOut(); this.getDataFromLocal()}}
                                        disabled={this.state.loggedin === "false" || !sessionStorage.getItem('loggedin')}>
                                    Log out
                                </Button>
                                <Dropdown.Divider/>
                                <Link to="/users" hidden={this.state.loggedin !== true && this.state.role !== 'Admin'}>
                                    <Button variant="outline-dark" style={{width: '10rem'}}>User management</Button>
                                </Link>
                            </Dropdown.Menu>
                        </Dropdown>
                    </Nav>

                    </Navbar>
            </div>
            
        );
    }
}

export default NavBar;