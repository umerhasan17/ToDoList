import React , { Component } from "react";
import ToDos from "./toDosContainer";
import "./toDoList.css";

class TodoList extends Component {

    constructor(props) {
        super(props);
        // arrow functions avoid the need to bind this to the specific functions
        this.state = {
            items: []
        };
    }

    addToDo = (item) => {
        var newItem = {
            text: this._inputElement.value,
            // key value unique enough for now
            // TODO: create a unique key value hash for each todo
            key: Date.now()
        };
        
        // modifying the state using this function is better than mutating directly
        this.setState((prevState) => {
            return {
                items: prevState.items.concat(newItem)
            };
        });

        this._inputElement.value = "";

        console.log(this.state.items);

        item.preventDefault();
    }

    deleteToDo = (key) => {
        // simply filtering out the todo with specific key value
        this.setState((prevState) => {
            return {
                items: prevState.items.filter(function(item) {
                    return (item.key !== key)
                })
            };
        });
    }

    render() {
        return (
            <div className="todoListMain">
                <div className="header">
                    <span><form onSubmit={this.addToDo}>
                        {/* ref keyword to pass values up into the addItem function */}
                        <input ref={(a) => this._inputElement = a} placeholder="enter task">
                        </input>
                        <button type="submit">Add Quick To Do</button>
                    </form>
                    <button style={{float : "right"}}>Add Full To Do</button></span>
                </div>
                {/* pass items array down into ToDos to create the list 1 by 1*/}
                <ToDos toDoList={this.state.items} delete={this.deleteToDo} />
            </div>
        );
    }
}

export default TodoList;