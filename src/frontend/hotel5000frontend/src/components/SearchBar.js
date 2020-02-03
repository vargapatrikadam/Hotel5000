import React, {Component} from 'react';
import {Button, FormControl, InputGroup} from "react-bootstrap";
import {FaSearch} from 'react-icons/fa';
import './SearchBar.css';

class SearchBar extends Component {
    render() {
        return (
            <div id="searchcontainer" className="bordered">
                <InputGroup id="input">
                    <FormControl size="lg" placeholder="Place for holiday"></FormControl>
                    <div as={InputGroup.Append} className="innerdiv">
                        <h5 className="label">From:</h5>
                    </div>
                    <input type="date" as={InputGroup.Append}/>
                    <div as={InputGroup.Append} className="innerdiv">
                        <h5 className="label">To:</h5>
                    </div>
                    <input type="date" as={InputGroup.Append}/>
                    <Button variant="light" as={InputGroup.Append}><FaSearch/>Search</Button>
                </InputGroup>
            </div>
        );
    }
}

export default SearchBar;