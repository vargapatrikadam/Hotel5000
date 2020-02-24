import React from 'react';

import Home from './pages/Home';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Lodgings from "./pages/Lodgings";
import Login from './pages/Login';
import Register from './pages/Register';
import About from './pages/About';
import Users from "./pages/Users";

function App() {
    return(
        <>
            <Router>
                <Switch>
                    <Route exact path="/" component={Home} />
                    <Route exact path="/lodgings" component={Lodgings} />
                    <Route exact path="/login" component={Login} />
                    <Route exact path="/register" component={Register}/>
                    <Route exact path="/about" component={About}/>
                    <Route exact path="/users" component={Users}/>
                    <Route component={Error} />
                </Switch>
            </Router>

        </>
    );
}

export default App;