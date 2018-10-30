import React from "react";

class ToDos extends React.Component {

  constructor(props) {
      super(props);

      this.createTasks = this.createTasks.bind(this);
  }

  createTasks(item) {
    return <li onClick={() => this.delete(item.key)} key={item.key}>{item.text}</li>;
  }

  delete(key) {
      this.props.delete(key);
  }

  render() {
    var todoEntries = this.props.entries;

    // map function to iterate over every entry passed in and add it to the list
    var listItems = todoEntries.map(this.createTasks);

    return <ul className="theList">{listItems}</ul>;
  }
}

export default ToDos;
