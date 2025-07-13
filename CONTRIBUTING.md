### Kanban Board
[Link to Project](https://github.com/users/MentIin/projects/2/views/11)

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
