# Metapolis

## Usage
[Explain how to use your MVP v2. Provide access instructions, authentication credentials if needed, etc.]

## Architecture
### Static View
[Document static architecture using UML Component diagram. Discuss coupling, cohesion, and maintainability.]

### Dynamic View
[Document dynamic architecture using UML Sequence diagram for a non-trivial request. Include performance metrics.]

### Deployment View
[Document deployment architecture with diagrams. Explain deployment choices.]

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
[Link to quality-attribute-scenarios.md]  
Documented quality characteristics from ISO 25010:
1. [Characteristic 1]
2. [Characteristic 2]
3. [Characteristic 3]

### Automated Tests
#### Tools Used:
- [Testing framework 1] for unit tests
- [Testing framework 2] for integration tests

#### Test Locations:
- Unit tests: [path]
- Integration tests: [path]

### User Acceptance Tests
[Link to user-acceptance-tests.md]

## Build and Deployment
### Continuous Integration
[Link to CI workflow file]  
Tools:
- [Static analysis tool 1] - [purpose]
- [Testing tool 1] - [purpose]
[Link to workflow runs]
