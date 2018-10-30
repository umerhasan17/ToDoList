import React from "react";

class ToDos extends React.Component {
  createTasks(item) {
    return <li key={item.key}>{item.text}</li>;
  }

  render() {
    var todoEntries = this.props.entries;

    // map function to iterate over every entry passed in and add it to the list
    var listItems = todoEntries.map(this.createTasks);

    return <ul className="theList">{listItems}</ul>;
  }
}

export default ToDos;
