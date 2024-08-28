using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.Charmap.Gui
{
    public class HelloWorldView : Custom
    {
        /// <summary>
        /// Example of templated control: the generic locator template (common for different places/ pages) 
        /// is specified for the control type via <see cref="FindTemplateAttribute"/>, 
        /// and for each specific place only unique part (label text for example) 
        /// should be specified via <see cref="FindParamAttribute"/>
        /// </summary>
        [Name("'Title' dropdown"), ById("titleCbox")]
        public Dropdown TitleDropdown { get; set; }

        /// <summary>
        /// Control of any type derived from <see cref="WebControl"/> could be a part of page object.<br/>
        /// Controls implementing predefined controls interfaces allow to apply type specific matchers 
        /// to make tests and assertions easier and more readable.
        /// </summary>
        [Name("'Name' input"), ById("nameInput")]
        public TextInput NameInput { get; set; }

        /// <summary>
        /// Controls could be located using generic <see cref="FindAttribute"/>.
        /// </summary>
        [Name("'Say' button"), ByName("Say Hello World")]
        public Button SayButton { get; set; }
    }
}
