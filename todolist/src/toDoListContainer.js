import React , { Component } from "react";
import ToDos from "./toDosContainer";

class TodoList extends Component {

    constructor(props) {
        super(props);
        // binding this to addItem to avoid using arrow functions
        this.addItem = this.addItem.bind(this);

        this.state = {
            items: []
        };
    }

    addItem(item) {
        if (this._inputElement.value !== "") {
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
    }

    render() {
        return (
            <div className="todoListMain">
                <div className="header">
                    <form onSubmit={this.addItem}>
                        {/* ref keyword to pass values up into the addItem function */}
                        <input ref={(a) => this._inputElement = a} placeholder="enter task">
                        </input>
                        <button type="submit">add</button>
                    </form>
                </div>
                {/* pass items array down into ToDos to create the list 1 by 1*/}
                <ToDos entries={this.state.items} />
            </div>
        );
    }
}

export default TodoList;