using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace ToDoList.Models
{
    public class ToDo
    {
        public int ToDoID { get; set; }

        public string Description { get; set; }

        [DisplayName("Complete")]
        public bool IsDone { get; set; }

        // Question mark after the datatype means that the value can be set to null

        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }

        [DisplayName("Remind Me")]
        public DateTime? ReminderDate { get; set; }

        [DisplayName("Priority Level")]
        public int? PriorityLevel { get; set; }

        [DisplayName("Latitiude")]
        public decimal? Lat { get; set; }

        [DisplayName("Longitude")]
        public decimal? Lon { get; set; }

        public TimeSpan? Duration { get; set; }

        [DisplayName("Dependent Tasks")]
        public List<int> Dependencies { get; set; }

        // Below are the navigation properties assocaited with this class. They provide a way to navigate an association between 2 entity types. Every foreign key has a corresponding navigation property. 

        public virtual ApplicationUser User { get; set; }

        public virtual List List { get; set; }

        public virtual Folder Folder { get; set; }

    }

    public class List
    {
        public int ListID { get; set; }

        [DisplayName("List Name")]
        public string ListName { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Folder Folder { get; set; }

        // Foreign keys from 1 to many relationships need to be stored as an ICollection or another type of list since many todos can be assigned to one list. 

        public virtual ICollection<ToDo> ToDos { get; set; }

    }

    public class Folder
    {
        public int FolderID { get; set; }

        [DisplayName("Folder Name")]
        public string FolderName { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<List> Lists { get; set; }

    }

}