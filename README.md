## Development
### Kanban board
Link: https://github.com/users/MentIin/projects/2/views/11
#### Entry criteria for columns:
- **To do** - issue (task) will be resolved during this sprint. It is the Sprint Backlog
- **In progress** - issue is currently being worked on. The issue is here if a developer is currently working on it.
- **Review (Testing)** - issue is completed and is being tested for bugs (if it works as expected)
- **Review (QA)** - issue was tested successfully and is being testing for quality (if they are balanced)
- **Done** - issue is fully completed/resolved and tested
### Git workflow
This project uses **GitFlow**.
**GitFlow** is a structured workflow using two primary branches - `main` (production) and `development` (integration) - alongside feature branches (for new work). Features branch off `development`, and merge back when done. When a the `development` branch mathes the release requirements, it is merged into `main`. Branch names should follow kebab-case. Old branches use PascalCase.
#### Issue templates
**Earlier templates:** no issue templates were used. Issues didn't have labels, most suitable people were assigned.
##### User story
Given ..., when ..., then ...
##### Bug report
Current behaviour:
Expected beahaviour:
Reproduction steps:
##### Technical task
Subtasks: ...

#### Pull Request
We use PRs only for merging. Commiting to feature branches doesn't require a PR.
##### Template
**Earlier templates:** deafault GitHub PR request was used ("Merge `branch1` into `branch2`").
Feature ... completed
The PR should contain all closed issues in its merge commit.
##### Merging PRs
#### Issue details
##### Labels
- **bug** - something isn't working, a bug that needs to be fixed
- **documentation** - improvements or additions to documentation
- **enhancement** - new feature or request
- **help wanted** - extra attention is needed, assign another team member
- **invalid** - this doesn't seem right, balancing issues

##### Assignees
People are assigned to issues at the start of the Sprint (at a meeting). If they want, they can assign themselves to other issues themselves.
#### Commits
Commits should start with an action (verb in present simple) and a subject of this action. Example: Add rover model, Fix unit test, Add inertia to player controller.
#### Code reviews
The code should be reviewed in the process of closing the PR. If something is wrong the comment should be added to the PR, telling the assignee about the problem.
### Secrets Managements
The project doesn't have any passwords or API keys.


