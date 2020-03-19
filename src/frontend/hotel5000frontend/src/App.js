import React from 'react';

import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Lodgings from "./pages/Lodgings";
import Login from './pages/Login';
import Register from './pages/Register';
import About from './pages/About';
import Users from "./pages/Users";
import PostLodging from "./pages/PostLodging";
import OwnReservationsPage from "./pages/OwnReservationsPage";
import OwnLodgings from "./pages/OwnLodgings";

function App() {
    return(
        <>
            <Router>
                <Switch>
                    <Route exact path="/" component={Lodgings} />
                    <Route exact path="/login" component={Login} />
                    <Route exact path="/register" component={Register}/>
                    <Route exact path="/about" component={About}/>
                    <Route exact path="/users" component={Users}/>
                    <Route exact path="/postLodging" component={PostLodging}/>
                    <Route exact path="/ownReservations" component={OwnReservationsPage}/>
                    <Route exact path="/ownLodgings" component={OwnLodgings}/>
                    <Route component={Error} />
                </Switch>
            </Router>

        </>
    );
}

export default App;