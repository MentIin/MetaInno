# Metapolis

## Usage
To access Metapolis, visit the Itch.io page (https://innopians.itch.io/metapolis   password: inno), launch the browser-based game, and navigate using WASD (interact with Space/E). For issues, contact @DirectX_11. There is a possibility that the server is off or malfunctioning (since we use free VM), contact us if so.

## Architecture
### Static View
![alt text](https://github.com/MentIin/MetaInno/blob/read-me-enhance/.github/static_view.png)
The team has focused on improving the map and polishing the player movement.

### Dynamic View
![alt text](https://github.com/MentIin/MetaInno/blob/read-me-enhance/.github/dynamic_view.png)

### Deployment View
The diagram for this process will be unnecessary.
We follow these steps:
1. We build the game inside the editor
2. Compress the resulted folder into a zip archive
3. Upload the zip to PlayFlow to launch the server
4. Upload the zip to Itch io to make the game available for clients

We introduced CD for our project, but its currently less effective than current approach.

## Development
### Kanban Board
Link: https://github.com/users/MentIin/projects/2/views/11

#### Entry Criteria for Columns:
- **To Do** - Issue is part of Sprint Backlog and will be resolved this sprint
- **In Progress** - Developer is actively working on the issue
- **Review (Testing)** - Code is complete and being tested for functionality
- **Review (QA)** - Functionality confirmed and being evaluated for quality
- **Done** - Issue is fully completed and meets all quality standards

### Git Workflow
This project uses **GitFlow** with two primary branches:
- `main` (production)
- `development` (integration)

#### Branching Strategy:
- Feature branches branch off `development`
- Branch names follow kebab-case (new) or PascalCase (legacy)
- Hotfix branches branch off `main`

#### Issue Management:
##### Templates:
Full templates can be found in .github folder.
- **User Story**:  
  Given [context], when [action], then [outcome]
- **Bug Report**:  
  Current behavior: [description]  
  Expected behavior: [description]  
  Reproduction steps: [steps]
- **Technical Task**:  
  Subtasks: [checklist or links]

##### Labels:
- **bug** - Something isn't working
- **documentation** - Documentation improvements
- **enhancement** - New feature request
- **help wanted** - Requires additional attention
- **invalid** - Issue needs reevaluation

##### Assignees:
- Assigned during sprint planning
- Self-assignment allowed for additional work

#### Pull Requests:
- Required for all merges
- Must reference related issue
- Template includes:
  - Description of changes
  - List of closed issues
  - Testing performed

#### Code Reviews:
- Mandatory before merge
- Conducted via PR comments
- Must address all feedback before merging

#### Commit Messages:
- Start with action verb in present tense
- Example: "Add user authentication", "Fix navigation bug"

### Secrets Management
No sensitive credentials or API keys are currently stored in the project.

## Quality Assurance
### Quality Attribute Scenarios
[Link to quality-attribute-scenarios.md](https://github.com/MentIin/MetaInno/blob/read-me-enhance/docs/quality-assurance/quality-attribute-scenarios.md)

Documented quality characteristics from ISO 25010:
1. Reliability (Availability)
2. Performance Efficiency (Time Behaviour)
3. Security (Resistance)

### Automated Tests
#### Tools Used:
- **Unity Testing Framework** for unit tests

#### Test Locations:
- Unit tests: https://github.com/MentIin/MetaInno/tree/development/Assets/_Project/CodeBase/Tests
- Integration tests: we don't integrate third-party services/libraries

### User Acceptance Tests
There aren't strict user acceptance tests defined since our customer relies on the creativity part of this project. The customer said that the main goal is to make the game fun and fun doesn't have a concrete definition - it is achieved through creative thinking and testing.

## Build and Deployment
### Continuous Integration
CI tool: https://github.com/MentIin/MetaInno/blob/main/.github/workflows/super-linter.yml
Tools:
- Super Linter - lints the C# code
- Uniy Cloud Build - builds the project when commiting in `main`

Workflow runs: https://github.com/MentIin/MetaInno/actions
