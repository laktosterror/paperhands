##  Quizzimo

![preview](https://github.com/laktosterror/Quizzimo/blob/main/QuizWPF/demo/demo.gif)

---
Quizzimo is a WPF and XAML application designed for creating, managing, and playing quiz games.
### User Features:

- Question Pack Management: Users can create, edit, and delete "Question Packs" containing multiple-choice questions. 
- Play Mode: Randomizes questions and answer options, includes a countdown timer, and provides immediate feedback on user responses.
- API Integration: Optionally imports questions from the Open Trivia Database API. 
- User-Friendly Interface: Features a clean layout with optional keyboard navigation.


### Technical Features:

- WPF and XAML: Utilizes Windows Presentation Foundation (WPF) for building the user interface with XAML for layout design.
- MVVM Architecture: Implements the Model-View-ViewModel design pattern for separation of concerns and improved maintainability.
- Data Binding: Employs data binding techniques to synchronize the UI with underlying data models.
- JSON Data Management: Saves and loads question packs in JSON format.
- Asynchronous Programming: Utilizes asynchronous operations for file handling and API calls.
- API Integration: Integrates with the Open Trivia Database API for importing questions.
- Randomization Logic: Implements algorithms for randomizing question and answer order during gameplay.
- Network State: Implements logic to handle internet connection and possible network loss. 