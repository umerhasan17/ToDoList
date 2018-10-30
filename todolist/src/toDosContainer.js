import React from "react";
import FlipMove from "react-flip-move"

class ToDos extends React.Component {

  renderTasks = (item) => {
    return <li onClick={() => this.delete(item.key)} key={item.key}>{item.text}</li>;
  }

  delete(key) {
      this.props.delete(key);
  }

  render() {
    // map function to iterate over every entry passed in and add it to the list
    var listItems = this.props.toDoList.map(this.renderTasks);

    return (
        <ul className="theList">
            <FlipMove duration={250} easing="ease-out">
                {listItems}
            </FlipMove>
        </ul>
    );
  }
}

export default ToDos;
