import React, {Component} from 'react';
import {Button, FormControl, InputGroup} from "react-bootstrap";
import {FaSearch} from 'react-icons/fa';

class SearchBar extends Component {
    render() {
        return (
            <div style={{width: '50rem'}} className="mx-auto mt-3">
                <InputGroup id="input">
                    <FormControl size="lg" placeholder="Place for holiday"></FormControl>
                    
                    <input type="date" as={InputGroup.Append}/>
                    
                    <input type="date" as={InputGroup.Append}/>
                    <Button variant="outline-dark" as={InputGroup.Append}><FaSearch/>Search</Button>
                </InputGroup>
            </div>
        );
    }
}

export default SearchBar;