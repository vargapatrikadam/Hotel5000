import React, {Component} from 'react';
import {Button, FormControl, InputGroup} from "react-bootstrap";
import {FaSearch} from 'react-icons/fa';
//import './SearchBar.css';

class SearchBar extends Component {
    render() {
        return (
            <div id="searchcontainer" className="bordered">
                <InputGroup id="input">
                    <FormControl size="lg" placeholder="Place for holiday"></FormControl>
                    
                    <input type="date" value="2019-01-01" as={InputGroup.Append}/>
                    
                    <input type="date" value="2020-01-01" as={InputGroup.Append}/>
                    <Button variant="outline-dark" as={InputGroup.Append}><FaSearch/>Search</Button>
                </InputGroup>
            </div>
        );
    }
}

export default SearchBar;