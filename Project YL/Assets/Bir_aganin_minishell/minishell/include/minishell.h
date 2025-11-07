/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   minishell.h                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/22 19:11:14 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/21 21:27:37 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#ifndef MINISHELL_H
# define MINISHELL_H

// color macros
# include "colors.h"

// structs header
# include "structs.h"

// libft
# include "../libft/include/libft.h"

// to use printf
# include <stdio.h>

// to use malloc, free, exit
# include <stdlib.h>

// to use write, usleep, fork, getcwd, chdir, unlink, execve, dup, dup2, 
// pipe, access, read, close, isatty, ttyname, ttyslot, ioctl
# include <unistd.h>

// for O_* constants
# include <fcntl.h>

// to use pid_t type
# include <sys/types.h>

// to use waitpid, wait, wait3, wait4
# include <sys/wait.h>

// to use readline functions
# include <readline/readline.h>
# include <readline/history.h>

// to use errno and ENOENT
# include <errno.h>

// to use opendir, readdir and closedir
# include <dirent.h>

// to use signal functions
# include <signal.h>

// to use stat, lstat, fstat
# include <sys/stat.h>

// to use tcsetattr, tcgetattr
# include <termios.h>

// to use strerror
# include <string.h>

// to use perror (also in stdio.h)
// to use getenv
# include <stdlib.h>

// to use termcap functions
# include <term.h>

// to use ioctl
# include <sys/ioctl.h>

// =============================== main =======================================

// Main interactive prompt loop.
// 1. Generates and displays prompt.
// 2. Reads user input.
// 3. Handles signals and special commands.
// 4. Processes valid input.
void			prompt(t_shell *shell);

// Core minishell processing function.
// 1. Tokenizes input string.
// 2. Stores tokens in shell structure.
void			minishell(char *input, t_shell *shell);

// Parses tokens into executable structure.
// 1. Performs synxtax analysis.
// 2. Builds abstract syntax tree (AST).
// 3. Executes commands if no errors.
void			parser(t_token_list *tokens, t_shell *shell);

// Validates if input contains non-whitespace characters.
int				handle_all_white_spaces(char *input);

// =============================== tokenizer ==================================

// Converts input string into a list of tokens.
// 1. Initializes token list structure.
// 2. Processes each character sequentially.
// 3. Handles words, operators and whitespaces.
// 4. Returns NULL on failure.
t_token_list	*tokenizer(char *input);

// Processes single input character and delegates handling.
// 1. Skips whitespace characters.
// 2. Handles operators via process_operator.
// 3. Processes words via handle_word.
// Returns 0 on error, 1 on success, 2 when operator handled.
int				process_input_char(char *input, int *i, t_token_list *tokens);

// Handles operator characters in input.
// 1. Delegates to handle_operators.
// 2. Returns appropriate status code.
int				process_operator(char *input, int *i, t_token_list *tokens);

// Processes and creates word tokens.
// 1. Finds word boundaries.
// 2. Handles quoted sections.
// 3. Creates and appends new word token.
int				handle_word(char *input, int *i, t_token_list *tokens);

// Handles quoted sections within words.
// 1. Detects quote start.
// 2. Skips until matching quote.
// 3. Validates proper quote termination.
int				handle_quotes(char *input, int *j);

// Handles operator characters in input.
// 1. Checks if current character is an operator.
// 2. Creates operator token if valid.
// 3. Reports errors for invalid operators.
// Returns 1 if operator handled, 0 on error, -1 if not operator.
int				handle_operators(char *input, int *i, t_token_list *tokens);

// Creates and appends operator token to list.
// 1. Determines operator type and length.
// 2. Extracts operator substring.
// 3. Creates and appends new token.
// Returns 1 on success, 0 if no operator found.
int				create_op_token(char *input, int *i, t_token_list *tokens);

// Checks for two-character operators.
// 1. Tests for >>, <<, && and ||.
// 2. Sets type and length if found.
// Returns 1 if match found, 0 otherwise.
int				is_two_char_op(char *input, t_token_type *type, int *len);

// Checks for single-character operators (part 1).
// 1. Tests for >, <, |.
// 2. Sets type and length if found.
// 3. Delegates remaining checks to is_one_char_op_2
// Returns 1 if match found, 0 otherwise.
int				is_one_char_op_1(char input, t_token_type *type, int *len);

// Checks for single-character operators (part 1).
// 1. Tests for (, ).
// 2. Sets type if found.
// Returns 1 if match found, 0 otherwise.
int				is_one_char_op_2(char input, t_token_type *type);

// Checks if a character exists in a string.
// 1. Iterates through string s to find character c.
int				in(const char *s, char c);

// Creates a new token.
// 1. Allocates a t_token.
// 2. Sets type and value.
// 3. Initializes next to NULL.
t_token			*new_token(t_token_type type, char *value);

// Initializes t_token list struct.
void			token_list_init(t_token_list *list);

// Appends token to the end of the list.
// 1. Links new token to tail if list is not empty.
// 2. Updates head if list was empty.
// 3. Updates tail and increments size.
void			token_list_append(t_token_list *list, t_token *token);

// Frees all tokens in the list and the list itself.
// 1. Iterates through each token.
// 2. Frees token value and token struct.
// 3. Frees the list structure.
void			token_list_free(t_token_list *list);

// =============================== env ========================================

// Initializes a linked list of environment variables from envp.
// 1. Iterates over each entry in envp.
// 2. Process each entry with env_process() to create a node.
// 3. Returns the fully constructed list.
t_env			*init_env(char **envp);

// Process a single environment variable string.
// 1. Splits the string into key and value at '=" character.
// 2. Creates and appends a new node to the list.
int				env_process(char *entry, t_env **head, t_env **tail);

// Creates new environment variable node.
// 1. Allocates and initializes a new t_env node.
// 2. Duplicates key and value strings.
t_env			*env_new(const char *key, const char *value);

// Appends a node to the end of the t_env list.
// 1. Adds to head if list is empty.
// 2. Otherwise appends to tail.
int				env_append(t_env **head, t_env **tail, t_env *node);

// Creates empty environment variable if nonexistent.
// 1. Checks if variable already exists.
// 2. Appends new node with NULL value if not found.
void			env_export_only(t_env **env_head, char *name);

// Retrieves value for environment variable key.
char			*get_env_value(t_env *env, const char *key);

// Gets PATH environment variable value.
char			*get_env_path(t_env *env);

// Updates existing environment variable or creates new.
// 1. Finds and updates matching key.
// 2. Creates new entry if key was not found.
void			env_update(t_env **env, char *key, char *value);

// Creates and appends new environment variable.
void			env_createapnd(t_env **head, char *key, char *val, t_env *curr);

// Converts environment linked list to a NULL-terminated array of strings.
// 1. Counts exported variables (those with values).
// 2. Allocates and fills array with "KEY=VALUE" strings.
// 3. Returns the array for use with execve.
// Note: Exits on malloc or ft_strjoin3 failure.
char			**env_list_to_array(t_env *env);

// Frees a NULL-terminated array of strings.
// 1. Frees each string in the array.
// 2. Frees the array pointer itself.
void			free_string_array(char **arr);

// =============================== clean ======================================

// Cleans up all shell resources.
// 1. Clears command history.
// 2. Frees token list if exists.
// 3. Frees AST if exists.
// 4. Frees environment variables list if exists.
void			clean_all(t_token_list *tokens, t_ast *node, t_env **env);

// Reports unexpected quote syntax error.
// 1. Prints to standard error prefix.
// 2. Displays the problematic quotes character.
// 3. Adds formatting suffix.
void			report_unexpected_quotes(const char token_value);

// Reports general syntax error and returns failure.
// 1. Prints to standard error prefix.
// 2. Displays the unexpected token.
// 3. Adds formatting suffix.
// Returns 0 to indicate error.
int				report_unexpected(const char *token_value);

// Frees entire environment variable list.
// 1. Iterates through each node.
// 2. Frees key, value and node struct.
// 3. Sets head pointer to NULL.
void			env_free_all(t_env **head);

// Recursively frees the entire AST.
// 1. Recursively frees left and right branches.
// 2. Frees command node if present.
// 3. Frees current AST node.
void			ast_free(t_ast *root);

// Frees command structure and its contents.
// 1. Closes heredoc FD if open.
// 2. Frees argument list.
// 3. Frees redirection list.
// 4. Frees the command struct.
void			command_free(t_command *command);

// Frees redirection list and associated resources.
// 1. Closes heredoc FD if applicable.
// 2. Frees redirection filename.
// 3. Frees redirection node.
void			free_redirs(t_redir *redir);

// Frees all arguments of a command.
// 1. Frees each argument string.
// 2. Frees the argument array.
void			free_args(t_command *command);

// Frees a null-terminated array of pointers.
// 1. Frees each element in the array.
// 2. Frees the array itself.
void			free_array(void **ptr);

// Frees a null-terminated array of strings.
// 1. Frees each string in the array.
// 2. Frees the array itself.
// Note: Specialized for char ** arrays.
void			free_args_temp(char **args);

// Frees a linked list of argument nodes.
// 1. Frees each string in the list.
// 2. Frees each list node.
void			free_args_list(t_args *args);

// =============================== expansion ==================================

// Main function for expanding environment variables and wildcards.
// 1. Returns early if args is empty or NULL.
// 2. Expands environment variables using 'expand_env()'.
// 3. Applies wildcard expansion on the result with 'expand_wild()'.
// 4. Converts the resulting linked list into a string array.
// 5. Frees old args and replaces with expanded result.
void			expander(char ***args, t_env *env, int *size_args, t_shell *sh);

// Converts a liked list of arguments (t_args) to a NULL-terminated string
// array.
// 1. Counts size of the linked list.
// 2. Allocates a new array of size + 1.
// 3. Duplicates each string into the array using ft_strdup().
// 4. Sets last element to NULL and returns the array.
char			**list_to_args(t_args *args, int *size_args);

// Expands environment variables from a string array of arguments.
// 1. Starts expansion with the first valid token.
// 2. Concatenates all expanded tokens into a linked list (t_args).
// 3. Returns the list of expanded arguments.
t_args			*expand_env(char **args, t_env *env, t_shell *sh);

// Extracts the prefix (before '$') from input, respecting quote context.
// 1. Scans input until '$' or null terminator (except inside single quotes).
// 2. Builds prefix string character-by-character.
// 3. Updates quote state for each character scanned.
char			*start_prefix(char *input, int *i, t_quote *state);

// Expands a variable key and appends its value to the current prefix.
// 1. Looks up the variable in the environment.
// 2. Skips empty values.
// 3. Appends the value to 'exp->prefix'.
// 4. If not quoted, splits and adds tokens individually.
void			expand_variable(t_exp *exp, char *key);

// Expands the special variable '$?' using the last exit status.
// 1. Converts status to string.
// 2. Appends it to 'exp->prefix'.
void			expand_status(t_exp *exp, int status);

// Handles variable expansion with no quotes (e.g., '$VAR').
// 1. Splits prefix by spaces.
// 2. If multiple parts, adds all but the last as individual args.
// 3. The last part is kept as prefix for continued parsing.
void			handle_no_quotes(t_exp *exp);

// Extracts an environment variable key starting at '$'.
// 1. Starts after '$' and reads alphanumeric or '_' characters.
// 2. Returns a new string with the extracted key.
char			*extract_key(char *input, int *i);

// Updates the current quote state based on the input character.
// Handles transitions between NO_QUOTE, SINGLE_QUOTE, and DOUBLE_QUOTE.
void			update_state_quote(t_quote *state, char c);

// Finds the value for a given environment variable key.
// 1. Iterates through the env list.
// 2. Returns a strdup of the value if found, or an empty string otherwise.
char			*find_env_value(char *key, t_env *env);

// Joins two strings and frees both.
// 1. Handles NULL safely.
// 2. Frees both inputs regardless of which one is NULL.
// 3. Returns the newly joined string.
char			*ft_strjoin_free(char *s1, char *s2);

// Expands environment variables and tokens in the input string.
// 1. Initializes the expander state.
// 2. Processes the input string token by token.
// 3. Adds any remaining prefix as a token.
// 4. Returns the linked list of expanded tokens.
t_args			*expand_token(char *input, t_env *env, t_shell *sh);

// Handles the next token during expansion.
// 1. If token starts with '$', handles special cases or extracts key and
// expands variable.
// 2. Otherwise, appends the next prefix substring.
// 3. Advances parsing index accordingly.
void			handle_next_token(t_exp *exp, char *input, t_shell *sh);

// Handles special cases for '$' expansions where next char is non-variable.
// 1. Checks if next character is end, space, quote, or '/'.
// 2. Appends a literal '$' to prefix.
// 3. Advances the parsing index.
// 4. Returns 1 if handled, 0 otherwise.
int				handle_special_expand(t_exp *exp, char *input);

// Initializes the expander structure.
// 1. Sets initial index and quote state.
// 2. Allocates token list head pointer.
// 3. Initializes prefix by extracting starting characters.
// 4. Sets environment pointer.
void			init_expander(t_exp *exp, char *input, t_env *env);

// Adds a new node with the given value to the end of the linked list.
void			add_token(t_args **head, char *value);

// Adds a token and frees the given value.
void			add_token_free(t_args **head, char *value);

// Appends a list of tokens to the end of another list.
// 1. If head is empty, assigns list to head.
// 2. Otherwise, finds the last node and appends the list.
void			append_list(t_args **head, t_args *list);

// Counts number of elements in a NULL-terminated array.
// 1. Iterates through the array until a NULL is found.
// 2. Returns the count.
int				count_array(void **arr);

// Expands wildcards in a linked list of arguments.
// 1. Iterates over each argument.
// 2. If argument contains wildcard, parses pattern and finds matches.
// 3. If no matches, adds argument without quotes.
// 4. If matches found, appends them sorted to the result list.
// 5. Returns the expanded list.
t_args			*expand_wild(t_args *envs);

// Parses a wildcard pattern into parts for matching.
// 1. Checks if argumennt is only asterisks and sets full flag.
// 2. Otherwise splits argument by wildcards into parts.
// 3. Sets flags indicating if pattern has fixed start or end.
// 4. Cleans empty parts if necessary.
// 5. Returns the pattern struct.
t_wild			parse_pattern(char *arg);

// Finds filenames in the current directory matching the wildcard pattern.
// 1. Opens current directory.
// 2. Iterates over directory entries.
// 3. Skips hidden files unless pattern explicitly starts with '.'.
// 4. Adds matching entries to the result list.
// 5. Returns list of matched filenames.
t_args			*wild_matches(t_wild *wild);

// Checks if a string contains an unquoted wildcards character.
// 1. Iterates over string characters updating quotes state.
// 2. Returns 1 if unquoted '*' found, else 0.
int				is_wildcard(char *arg);

// Checks if a string consists only of asterisks.
// 1. Iterates through string characters.
// 2. Returns 0 on any non-asterisk character found.
// 3. Returns 1 if all characters are '*'.
int				only_asterisk(char *arg);

// Matches all middle parts of a wildcard pattern in a string.
// 1. Iterates from index 'i' to 'limit' in the parts array.
// 2. Uses ft_strnstr to check if each part appears in order in 'pos'.
// 3. Advances the pointer past each match.
// 4. Returns 0 on failure, 1 if all parts match in order.
int				match_middle(char *pos, t_wild *wild, int i, int limit);

// Checks if a string ends with a giben suffix.
// 1. Gets lengths of filename and suffix.
// 2. Returns 0 if filename is too short.
// 3. Uses ft_strcmp to compare suffix with the end of filename.
int				match_end(const char *filename, const char *end);

// Matches a filename against a wildcard pattern.
// 1. Returns true if pattern is a full wildcard (*).
// 2. Handles NULL checks and calculates number of pattern parts.
// 3. If pattern has a start, checks and skips it.
// 4. Verifies all middle parts appear in order.
// 5. If pattern has an end, ensures filename ends with it.
// 6. Returns 1 if pattern matches, 0 otherwise.
int				match_pattern(const char *filename, t_wild *wild);

// Handles opening of quotes in input string.
// 1. If single or double quote is found and state is NO_QUOTE, updates state.
// 2. Increments index.
// 3. Returns 1 if a quote was handled, 0 otherwise.
int				handle_opening_quote(char *input, int *i, t_quote *state);

// Updates quote state while iterating through the input string.
// 1. Handles both opening and closing of quotes.
// 2. Advances index as needed.
// 3. Returns 1 if a quote was handled, 0 otherwise.
int				update_state_quote_i(char *input, int *i, t_quote *state);

// Splits a wildcard pattern string into parts separated by unquoted asterisk.
// 1. Initializes state and indices.
// 2. Iterates over the input string updating quote state.
// 3. On unquoted '*', extracts substring between indices and adds parts array 
// after removing quotes.
// 4. Adds final substring after loop if exists, or adds empty string if none
// found.
// 5. Returns array of substrings.
char			**split_wildcard(char *input);

// Initializes variables used in split_wildcard().
void			init_vars_wild(t_quote *state, char ***parts, int *i, int *j);

// Adds a new token string to a dinamically growing array of strings.
// 1. Calculates current length of the array.
// 2. Allocates a new array with space for the new token and NULL terminator.
// 3. Copies existing pointers to new array.
// 4. Adds new token pointer at the end and NULL terminator.
// 5. Frees old array and updates pointer to new array.
void			add_token_to_array(char ***array, char *token);

// Swaps the 'arg' strings between two t_args nodes.
// 1. Temporarily stores a->arg.
// 2. Assigns b->arg to a->arg.
// 3. Assigns temp to b->arg.
void			swap_args(t_args *a, t_args *b);

// Converts uppercase character to lowercase.
// 1. Returns lowercase equivalent if character is in 'A'-'Z'.
// 2. Otherwise, returns character unchanged.
int				ft_tolower(int c);

// Compares two strings ignoring case.
// 1. Iterates through both strings.
// 2. Compares characters after converting to lowercase.
// 3. Returns difference on first mismatch, or 0 if equal.
// Note: Case-insensitive version of ft_strcmp().
int				ft_strcasecmp(const char *s1, const char *s2);

// Sorts a t_args linked list in ascending order (case-insensitive).
// 1. Iteratively compares adjacent nodes.
// 2. Swaps arguments if out of order.
// 3. Repeats until no swaps occur.
// Note: Uses bubble sort, suitable for small lists.
void			sort_args_list(t_args *list);

// Sorts a list and appends it to the head list.
// 1. Sorts the list using sort_args_list.
// 2. Appends the sorted list using append_list.
void			sort_append(t_args **head, t_args *list);

// Expands all redirections except heredocs.
// 1. Iterates through all redirections.
// 2. Calls expand_redir on each except type R_HEREDOC.
void			expand_redirs(t_redir *redirs, t_shell *sh);

// Expands wildcards in a single redirection's filename.
// 1. Converts filename into argument list.
// 2. Expands wildcards in the argument list.
// 3. Counts matches and handles:
// 	- No match: calls handle_no_match().
// 	- Multiple matches: calls handle_ambigous().
// 	- Single match: calls handle_single_match().
// 4. Frees temporary argument lists.
void			expand_redir(t_redir *redir, t_shell *sh);

// Expands wildcards in an argument list.
// 1. Iterates over args and detects wildcards.
// 2. Parses wildcards patterns and finds matches.
// 3. Sorts and appends matches to result list.
// 4. Removes quotes from non-wildcard args and adds them directly.
// 5. Returns the expanded argument list or NULL on failure.
t_args			*expand_redir_wild(t_args *envs);

// Counts the number of elements in a linked list of arguments.
// 1. Iterates through the list.
// 2. Increments count for each node.
// 3. Returns the total count.
int				count_args(t_args *list);

// Handles the case when no matches are found for a redirection filename.
// 1. Sets shell last_status to 1.
// 2. Prints error message with filename.
// 3. Sets redirection filename to NULL to indicate failure.
void			handle_no_match(t_shell *sh, t_redir *redir);

// Handles the case when a redirection filename expansion is ambigous
// (multiple matches).
// 1. Sets shell last_status to 1.
// 2. Prints ambiguous redirect error with filename.
// 3. Sets redirection filename to NULL to indicate failure.
void			handle_ambiguous(t_shell *sh, t_redir *redir);

// Handle the case when a redirection filename expands to a single match.
// 1. Frees old filename string.
// 2. Replaces it with the expanded filename string.
void			handle_single_match(t_redir *redir, t_args *expanded_list);

// =============================== utils ======================================

// Handles fatal errors with system message.
// 1. Prints error message to stderr.
// 2. Exits with failure status.
void			exit_perror(const char *msg);

// Removes quotes from the input string, respecting quote context.
// 1. Allocates memory for the resulting string.
// 2. Initializes variables and quotes state.
// 3. Iterates through input, skipping handled quotes.
// 4. Copies characters not part of outer quotes to result.
// 5. Frees original string if free_str is true.
// 6. NULL-terminates and returns the result.
// Note: Respects nested an matched quote rules.
char			*rem_quotes(char *str, int free_str);

// Initializes loop indexes and quote state.
void			init_vars_remove_quotes(int *i, int *j, t_quote *state);

// =============================== ast ========================================

// Parses logical AND/OR operations into AST.
// 1. Parses left-hand side using parse_pipe().
// 2. Loops over logical operators (||, &&) and builds tree.
// 3. Creates new AST nodes for each logical operation.
// Note: Left-associative for multiple logical operators.
t_ast			*parse_logical(t_token **token, t_shell *shell);

// Parses pipe operations into AST.
// 1. Parses left-hand side using parse_simple_command().
// 2. Loops over PIPE tokens and builds binary tree.
// 3. Updates token pointer after parseing.
t_ast			*parse_pipe(t_token **token, t_shell *shell);

// Parses either a command or a subshell.
// 1. If token is PAREN_CLOSE, calls parse_subshell.
// 2. Otherwise calls parse_command.
t_ast			*parse_simple_command(t_token **token, t_shell *shell);

// Parrses a subshell into an AST node.
// 1. Skips opening parenthesis.
// 2. Parses inner expression with parse_logical().
// 3. Tracks nested parenthesis until closing.
// 4. Creates subshell node wrapping the subtree.
t_ast			*parse_subshell(t_token **token, t_shell *shell);

// Parses a simple command into an AST node.
// 1. Creates NODE_COMMAND AST node.
// 2. Populates node with parsed command via make_command().
t_ast			*parse_command(t_token **token, t_shell *shell);

// Checks if token is a pipe, logical operator, or parenthesis.
// 1. Returns 1 for PIPE, LOGICAL_OR, LOGICAL_AND.
// 2. Returns 1 for PAREN_OPEN or PAREN_CLOSE.
// 3. Returns 0 otherwise.
int				is_pipe_or_logical(t_token *token);

// Checks if token is a redirection operator.
// 1. Returns 1 for redirections and 0 otherwise.
int				is_redirect(t_token *token);

// Calculates number of WORD arguments before control operators.
// 1. Iterates until pipe, logical or parenthesis token.
// 2. Increments counter for each WORD token.
// 3. Skips two tokens for redirection pairs.
int				get_size_args(t_token **token);

// Checks if string contains single or double quotes.
// 1. Iterates through string.
// 2. Returns 1 if ' or " is found and returns 0 otherwise.
int				have_quotes(char *raw);

// Constructs a t_command node from tokens.
// 1. Initializes command structure.
// 2. Allocates space for arguments.
// 3. Copies WORD tokens into args array.
// 4. Handles redirections via parse_redirect().
// 5. Ends args with NULL terminator.
// 6. Updates token pointer to next control operator.
// Note: Relies on get_size_args for pre-counting arguments.
t_command		*make_command(t_token **token, t_shell *shell);

// Initializes and returns a blank t_command struct.
// 1. Sets args and redirs to NULL.
// 2. Sets heredoc_fd to -1.
t_command		*init_command(void);

// Parses and appends a redirection to the command.
// 1. Checks if next token exists.
// 2. Handles HEREDOC using parse_heredoc.
// 3. Maps REDIR tokens to enum types.
// 4. Appends redirection and advances token pointer.
// Note: Skips two tokens for regular redirections (operator + file).
void			parse_redirect(t_token **token, t_command **cmd, t_shell *sh);

// Parses and adds a heredoc redirection to the command.
// 1. Creates a heredoc node if not interrupted by signal.
// 2. Appends it to the command's redirection list.
// 3. Advances token pointer by two (operator + delimiter).
// Note: Relies on global g_signal_status to skip heredoc on interrupt.
void			parse_heredoc(t_token **token, t_command **cmd, t_shell *shell);

// Creates a heredoc redirection node.
// 1. Extracts delimiter with or without quote expansion.
// 2. Processes heredoc input and gets temporary FD.
// 3. Allocates and fills t_redir node with type R_HEREDOC.
// 4. Sets filename, heredoc_fd and next pointer.
t_redir			*create_heredoc_node(char *raw, t_shell *shell);

// Removes quotes form heredoc delimiter.
// 1. Sets no_expand flag if quotes are present.
// 2. Returns duplicated string with quotes removed.
char			*strip_quotes(char *raw, int *no_expand);

// Appends a redirection node to the redir list.
// 1. If list is empty, sets head to node.
// 2. Otherwise, traverses to end and appends node.
void			build_redir_list(t_redir **head, t_redir *node);

// Creates and appends a redirection node (non-heredoc).
// 1. Duplicates and strips quotes from a filename.
// 2. Initializes t_redir with given type and filename.
// 3. Sets heredoc_fd to -1 and next to NULL.
// 4. Appends to redirection list.
// Note: Used for <, > and >> redirections.
void			append_redir(t_redir **head, t_redir_type type, char *filename);

// ======================== syntax analysis ===================================

// Performs syntax analysis on token list.
// 1. Checks for empty input.
// 2. Validates first token isn't a pipe/logical operator.
// 3. Analyzes each token in sequence.
// 4. Validates final token and parenthesis depth.
// Returns 1 if valid, 0 on syntax error.
int				syntax_analysis(t_token_list *tokens);

// Analyzes individual token in context.
// 1. Checks redirections have valid targets.
// 2. Validates pipe placement.
// 3. Validates logical operators.
// 4. Tracks parenthesis nesting.
// Returns 1 if valid, 0 on error.
int				tk_analysis(t_token *prev, t_token *tk, t_token *nxt, int *dpt);

// Checks if string contains only whitespace.
// 1. Iterates through string.
// 2. Returns 1 if all whitespace, 0 otherwise.
int				is_all_whitespace(const char *s);

// Validates pipe token placement.
// 1. Checks previous token is WORD or PAREN_CLOSE.
// 2. Checks next token is WORD, PAREN_OPEN, or any redirection.
// Returns 1 if valid, 0 on error.
int				check_pipe(t_token *prev, t_token *next);

// Validates redirection token placement.
// 1. Ensures redirection is followed by WORD.
// Returns 1 if valid, 0 on error.
int				check_redir(t_token *next);

// Validates logical operator placement.
// 1. Checks previous token is WORD or PAREN_CLOSE.
// 2. Ensures next token isn't another operator.
// Returns 1 if valid, 0 on error.
int				check_logical(t_token *prev, t_token *token, t_token *next);

// Checks if token is a two-character operator.
// Returns 1 for logical/redirection operators, 0 otherwise.
int				is_twochar(t_token *token);

// Validates parenthesis placement and nesting.
// 1. Checks opening parenthesis has valid content.
// 2. Validates closing parenthesis position.
// 3. Tracks nesting depth.
// Returns 1 if valid, 0 on error.
int				check_paren(t_token *p, t_token *t, t_token *n, int *depth);

// =============================== executor ===================================

// Executes AST nodes based on their type.
// 1. For NODE_COMMAND: expands arguments and executes the command.
// 2. For NODE_PIPE: executes the pipe sequence.
// 3. For NODE_AND: executes left node; if successful, executes right node.
// 4. For NODE_OR: executes left node; if failed and no signal interrupt,
// executes right node.
// 5. For NODE_SUBSHELL: executes the subshell node.
// Note: Recursively traverses AST to execute commands and control flow.
void			executor(t_token_list *tokens, t_shell *sh, t_ast *node);

// Executes a simple command node.
// 1. Expands redirections.
// 2. Attempts to handle exit builtin.
// 3. Attempts to handle other builtins.
// 4. Executes external commands if no builtin mathched.
void			exec_command(t_token_list *tokens, t_shell *shell, t_ast *node);

// Executes a pipe node connecting two commands.
// 1. Creates a pipe.
// 2. Forks two child processes for left and right nodes.
// 3. Handles each child's execution and pipes.
// 4. Closes pipe fds in parent.
// 5. Waits for both children and updates last status.
void			exec_pipe(t_shell *shell, t_ast *node);

// Executes a subshell node.
// Forks a child process to execute the subtree.
// Child sets up signals, execute subtree, cleans up, and exits.
// Parent waits for child and updates last_status.
// Note: Subshell executes in separate process context.
void			exec_subshell(t_token_list *tkns, t_shell *shell, t_ast *node);

// Checks if the given command is a shell builtin.
// Returns 1 if cmd is a builtin and 0 otherwise.
int				is_builtin(char *cmd);

// Executes a builtin command if it matches.
// 1. Dispaches the call to the appropriate builtin implementation.
// Returns the result of the builtin function.
// Returns 1 if the command is not a recognized builtin.
int				run_builtin(char **args, t_shell *shell);

// Checks if command is "exit" and runs builtin_exit if so.
// 1. Returns 1 if args is NULL or "exit" is detected.
// 2. Calls builtin_exit with toks and shell if "exit" is matched.
// 3. Returns 0 otherwise.
int				try_exit(t_token_list *toks, char **args, t_shell *shell);

// Runs built-in command with redirection if applicable.
// 1. Checks if command is a builtin.
// 2. Saves current stdin and stdout.
// 3. Applies redirection via apply_redirections().
// 4. Restores original fds if redirectoin fails.
// 5. Runs builtin and restores fds.
// 6. Updates last exit status.
// 7. Returns 1 if builtin was executed, 0 otherwise.
// Note: Manages redirections and builtin lifecycle.
int				try_other_builtin(char **args, t_command *cmd, t_shell *shell);

// Executes external command if it is valid and executable.
// 1. If path is an explicit directory, handles it with error.
// 2. Checks if command is executable.
// 3. Executes command if valid.
// 4. Handles permission denied or not found cases.
// Note: Dispatches final execution logic for non-builtins.
void			run_external_cmd(char **args, t_command *cmd, t_shell *shell);

// Checks if a command is an explicit directory path.
// 1. Verifies if cmd contains a '/' and refers to a directory via stat.
// 2. Returns 1 if cmd is a directory, 0 otherwise.
int				is_explicit_dir(char *cmd);

// Handles execution attempt on a directory.
// 1. Prints "Is a directory" error.
// 2. Sets last status to 126.
// Note: Called when a directory path is mistakenly executed.
void			handle_dir_case(char *cmd, t_shell *shell);

// Handles command not found errors.
// 1. If cmd contains '/', prints "No such file or directory".
// 2. Otherwise, prints "command not found".
// 3. Sets last status to 127.
void			handle_not_found(char *cmd, t_shell *shell);

// Handles permission denied errors.
// 1. Prints "Permission denied" message with command.
// 2. Sets last status to 126.
void			handle_permission_denied(char *cmd, t_shell *shell);

// Executes command with redirections applied.
// 1. Saves current stdin and stdout.
// 2. Applies redirections for the command.
// 3. Converts env list to array.
// 4. Calls exec_dispatch to execute command.
// 5. Frees env array and restore fds.
// 6. Sets last exit status and returns it.
// Note: Wraps redirection logic around external execution.
int				exec_with_redirs(char **args, t_command *cmd, t_shell *shell);

// Dispatches execution of a command (builtin or external).
// 1. Saves terminal settings if running interactivelly.
// 2. Runs builtin directly if applicable.
// 3. Sets signal handlers for execution.
// 4. Forks process and calls execve_with_path in child.
// 5. Waits for child process in parent.
// 6. Restores terminal settings and signal handlers.
// 7. Returns child's exit status.
// Note: Central logic for command executions flow.
int				exec_dispatch(char **args, t_shell *shell, char **envp);

// Searches for command in PATH and executes it.
// 1. Attempts direct execution if command contains '/'.
// 2. Splits PATH into directories.
// 3. Tries each candidate path using execve.
// 4. Validates each candidate before execution.
// 5. Exits with error if all candidates fail.
// Note: Final fallback for external commands.
void			execve_with_path(char **args, t_env *env, char **envp);

// Tries executing the command as an explicit path.
// 1. Checks for '/' in command.
// 2. If found, verifies access and runs execve.
// 3. Exits with error if access or execve fails.
void			try_exec_explicit(char *cmd, char **args, char **envp);

// Validates that the path is executable and not a directory.
// 1. Returns silently if path does not exist.
// 2. Exits if path is a directory.
// 3. Exits if path is not executable.
// Note: Prevents execution of invalid or restricted paths.
void			validate_exec_path(char *path);

// Checks if command is an executable file.
// 1. If command contains '/', checks if file exists, is not a directory, and
// is executable.
// 2. Otherwise, searches in PATH via search_in_path().
// 3. Returns 1 if executable, 0 if not found or not executable, -1 if
// permission denied.
// Note: Used to validate command before execve.
int				is_executable_command(char *cmd, t_env *env);

// Checks if command is an explicit path.
// 1. Returns 1 if command contains '/'.
// 2. Returns 0 otherwise.
// Note: Distinguishes between absolute/relative and bare commands.
int				is_explicit_executable(char *cmd);

// Searches PATH to check if command is executable.
// 1. Splits PATH into directoreis.
// 2. Appends command to each directory.
// 3. Uses access to check if candidate is executable.
// Returns 1 on success, 0 otherwise.
int				search_in_path(char *cmd, t_env *env);

// Handles execution logic for the left side of a pipe.
// 1. Sets execution signal handlers.
// 2. Redirects STDOUT to pipe write end.
// 3. Closes both pipe ends.
// 4. Executes the AST node.
// 5. Cleans up and exits with last status.
// Note: Used in the left child process of a fork during piping;
void			handle_left_child(int *fd, t_shell *shell, t_ast *node);

// Handles execution logic for the right side of a pipe.
// 1. Sets execution signal handlers.
// 2. Redirects STDIN to pipe read end.
// 3. Closes both pipe ends.
// 4. Executes the AST node.
// 5. Cleans up and exits with last status.
// Note: Used in the right child process of a fork during piping;
void			handle_right_child(int *fd, t_shell *shell, t_ast *node);

// Applies all I/O redirections for a command.
// 1. Iterates through redirection list in cmd.
// 2. Handles input redirection (R_IN) with handle_in_redir().
// 3. Handles heredoc (R_HEREDOC) with handle_heredoc.
// 4. Handles output/append redirection (R_OUT, R_APPEND) with handle_out_redir.
// 5. Stops on error and proceeds to finish_redirections.
// Note: All open FDs are managed and closed appropriately.
int				apply_redirections(t_command *cmd, t_shell *sh);

// Finalizes redirections by duplicating or closing FDs.
// 1. If error (ret < 0), closes any open FDs and returns -1.
// 2. If input FD is valid, redirects STDIN using dup2_close().
// 3. If output FD is valid, redirects STDOUT using dup2_close().
// Returns 0 on success.
int				finish_redirections(int ret, int in_fd, int out_fd);

// Handles input redirections from a file.
// 1. Opens a file in read-only mode.
// 2. Closes previously opened input FD, if any.
// 3. Stores new FD in *in_fd.
// Note: Returns -1 on failure, 0 on success.
int				handle_in_redir(t_redir *redir, int *in_fd, t_shell *sh);

// Opens a file and prints error to stderr on failure.
// 1. Tries to open file with given flags and mode.
// 2. On failure, prints error and sets last_status to 1.
// 3. Returns file descriptor or -1.
int				open_file_err(char *filename, int flags, int mode, t_shell *sh);

// Sets input file descriptor from heredoc.
// 1. Closes previously opened input FD, if any.
// 2. Uses heredoc_fd as new input.
// Note: Assumes heredoc_fd is already valid.
int				handle_heredoc(t_redir *redir, int *in_fd);

// Handles output or append redirection to a file.
// 1. Computes flags based on redir type.
// 2. Opens file with appropriate flags and mode.
// 3. Closes previously opened output FD, if any.
// 4. Stores new FD in *out_fd.
// Note: Returns -1 on failure, 0 on success.
int				handle_out_redir(t_redir *redir, int *out_fd, t_shell *sh);

// Replaces std_fd with fd and closes fd.
// 1. Duplicates fd to std_fd using dup2.
// 2. Closes original fd.
void			dup2_close(int fd, int std_fd);

// Saves the STDIN and STDOUT file descriptors.
// 1. Duplicates STDIN and STDOUT into save_stdin and save_stdout.
// 2. Exits on failure usint exit_perror().
// Returns 0 on success.
// Note: Used to restore I/O state after redirections.
int				save_fds(int *save_stdin, int *save_stdout);

// Restores STDIN and STDOUT from saved descriptors.
// 1. Replaces STDIN and STDOUT using dup2.
// 2. Closes the saved descriptors.
// 3. Exits on error using exit_perror().
// Returns 0 on success.
int				restore_fds(int save_stdin, int save_stdout);

// Sets the shell's last_status based on global signal status.
// 1. If a signal was received (g_signal_status >= 0), updates status to
// special value (-2).
// 2. Otherwise, store exit status's lower 8 bits.
void			set_last_status(t_shell *shell, int status);

// Returns the last stored shell status.
int				get_last_status(t_shell *shell);

// =============================== builtin ====================================

// Builtin implementation for the "exit" command.
// 1. Checks for too many arguments and errors out if present.
// 2. Parses the optional exit cod argument.
// 3. Frees all allocated resources.
// 4. Exits with the parsed code on error code.
// Note: Exits with status 2 on invalid numeric argument.
void			builtin_exit(t_token_list *tokens, t_shell *shell);

// Parses and validates the exit code argument.
// 1. Returns last shell status if no argument.
// 2. Validates the string contains only digits (with optional +/-).
// 3. Prints error and returns -1 on invalid input.
// 4. Returns code masked to 8 bits.
int				parse_exit_code(char *arg_str, t_shell *shell);

// Builtin impltmentation for the "pwd" command.
// 1. Attempts to get the PWD value from environment.
// 2. Prints PWD if found.
// 3. Otherwise, calls getcwd to get current working directory.
// 4. Prints current directory or error message on failure.
// 5. Frees allocated memory if getcwd is used.
int				builtin_pwd(t_env *env);

// Builtin implementation for the "env" command.
// 1. Iterates through the environment list.
// 2. Prints key=value pairs where value is not NULL.
int				builtin_env(t_env *env);

// Builtin implementation for the "echo" command.
// 1. Checks and skips all leading -n flags.
// 2. Prints all remaining arguments separated by spaces.
// 3. Appends a newline unless -n flag was provided.
int				builtin_echo(char **args);

// Builtin implementation for the 'cd' command.
// 1. Determines the target directory (args[1] or $HOME).
// 2. Attempts to change the current working directory.
// 3. If successful, updates OLDPWD and PWD in the environment.
// Returns 0 on succsess, 1 on failure.
int				builtin_cd(char **args, t_env *env);

// Builtin implementation for the "export" command.
// 1. If no arguments, prints current exported variables.
// 2. For each argument:
// 	- Finds '=' position.
// 	- Splits into name and value.
// 	- Validates and processes export argument.
// 3. Frees allocated name and value.
// 4. Returns 0 if all valid, 1 if any invalid argument.
int				builtin_export(char **args, t_env **env);

// Prints all environment variables in sorted order.
// 1. Counts environment variables.
// 2. Converts linked list to array.
// 3. Sorts the array alphabetically by key.
// 4. Prints variables in "declare -x key=value" format.
// 5. Frees the array.
void			print_exported(t_env *env);

// Builtin implementation for the "unset" command.
// 1. Iterates over all provided arguments.
// 2. Validates each argument and attempts to unset it from environment.
// 3. Tracks error status for invalid arguments.
// 4. Returns 0 if all valid, 1 if any invalid argument.
int				builtin_unset(char **args, t_env **env);

// =============================== signals ====================================

// Configures signal handlers for interactive prompt.
// 1. Sets up custom SIGINT handler with line editing support.
// 2. Ignores SIGQUIT signals.
void			setup_signals_prompt(void);

// Prepares SIGQUIT handler configuration for internactive prompt.
// 1. Sets custom handler function.
// 2. Initializes empty signal mask.
// 3. Enables system call restart flag.
t_sig			setup_sigint_prompt(void);

// Handles SIGINT during interactive prompt.
// 1. Sets global signal status to 130 (interrupt).
// 2. Prints newline and resets readline display.
// 3. Preserves prompt functionality.
void			handle_sigint_prompt(int sig);

// Updates shell status values.
// 1. Stores previous signal status in shell struct.
// 2. Updates global signal status with new value.
void			update_sh_last_status(t_shell *sh, int new_value);

// Handles EOF condition during heredoc input.
// 1. Prints warning message about missing delimiter.
// 2. Displays the expected delimiter.
void			handle_sigeof_heredoc(char *delim);

// Handles SIGINT during heredoc processing.
// 1. Sets global signal status to 130 (interrupt).
// 2. Prints newline for clean output.
void			handle_sigint_heredoc(int sig);

// Configures signal handlers for heredoc processing.
// 1. Disables terminal echo control characters.
// 2. Sets custom SIGINT handler.
// 3. Ignores SIGQUIT signals.
void			setup_signals_heredoc(void);

// Disables terminal echo control character display.
// 1. Gets current terminal attributes.
// 2. Clears ECHOCTL flag.
// 3. Applies new terminal settings.
void			disable_echoctl(void);

// Re-enables terminal echo control character display.
// 1. Gets current terminal attributes.
// 2. Sets ECHOCTL flag.
// 3. Applies new terminal settings.
void			enable_echoctl(void);

// Configures signal handlers for child processes.
// 1. Sets up SIGINT handler for graceful termination.
// 2. Configures SIGQUIT to use default behavior.
void			setup_signals_exec(void);

// Prepares SIGINT handles configuraton for child processes.
// 1. Sets custom handler function.
// 2. Initializes empty signal mask.
// 3. Clears special flags.
t_sig			setup_sigint_exec(void);

// Prepares SIGQUIT handler configuration for child processes.
// 1. Uses default system handler.
// 2. Initializes empty signal mask.
// 3. Clears special flags.
t_sig			setup_sigquit_exec(void);

// Handles SIGINT in child processes.
// 1. Sets exit status to 130. (standard bash interrupt code).
// 2. Prints newline for clean output.
void			handle_sigint_exec(int sig);

// Handles SIGQUIT in child processes. (using default behavior).
// 1. Sets exit status to 131. (standard bash quit code).
// 2. Prints core dump message.
void			handle_sigquit_exec(int sig);

// =============================== prompt =====================================

// Constructs interactive shell prompt string.
// 1. Gets user, hostname and current working directory.
// 2. Calculates required buffer size.
// 3. Assembles colored prompt components.
// Returns formatted prompt string.
char			*make_prompt(t_shell *shell);

// Assembles prompt components into formatted string.
// 1. Allocates buffer including space for color codes.
// 2. Builds user@host section.
// 3. Adds current working directory.
// 4. Appends prompt characters.
// Returns complete prompt string.
char			*assemble_prompt(char *user, char *host, char *cwd, size_t len);

// Gets curent working directory for prompt display.
// 1. Checks PWD environment variable.
// 2. Shortens path if inside home directory.
// 3. Fails back to system cwd if needed.
// Returns formatted directory string.
char			*get_cwd(t_shell *shell);

// Builds user@host section of prompt with color formatting.
// 1. Appends red color code.
// 2. Adds username and @ symbol.
// 3. Appends hostname.
// 4. Resets color.
void			build_user_host(char *dst, size_t *i, char *user, char *host);

// Builds current directory section of prompt with color formatting.
// 1. Appends colon separator.
// 2. Adds yellow color code.
// 3. Appends directory path.
// 4. Resets color.
void			build_cwd(char *dst, size_t *i, char *cwd);

// Appends string to destination buffer.
// 1. Copies source string to current position.
// 2. Updates position index.
void			append_str(char *dst, size_t *i, const char *src);

// Appends single character to destination buffer.
// 1. Stores character at current position.
// 2. Increments position index.
void			append_char(char *dst, size_t *i, char c);

// Gets current username from environment.
// 1. Checks USER environment variable.
// 2. Returns "unknown" if now found.
char			*get_user(void);

// Reads system hostname from /etc/hostname
// 1. Opens the hostname file.
// 2. Reads contents into buffer.
// 3. Remove trailing newline if present.
// 4. Returns "unknown" on error.
char			*read_hostname_file(void);

// Gets current working directory for display.
// 1. Retrieves full path using getcwd().
// 2. Shortens path if in home directory.
// 3. Returns "[deletes]" if directory inaccessible.
char			*get_cwd_display(void);

// Shortens home directory path to ~/ notation.
// 1. Calculates path segments after home.
// 2. Preserves trailint path components.
// 3. Returns "~" for empty home path.
char			*replace_home(char *cwd, char *home);

// Constructs shortened path string with ~ prefix.
// 1. Allocates space for ~, separator, and remainder.
// 2. Handles both with and without trailing path.
char			*build_shortc(char *src, char *cwd);

// =============================== heredoc ====================================

// Processes heredoc input until delimiter.
// 1. Creates pipe for heredoc output.
// 2. Reads lines with expansion handling.
// 3. Writes results to pipe.
// Returns read-end file descriptor.
int				process_hd(char *delim, int no_expand, t_env *env, t_shell *sh);

// Checks for heredoc termination conditions.
// 1. Matches exact delimiter.
// 2. Handles EOF without newline.
// 3. Handles NULL input (EOF).
// Returns 1 if heredoc should end, 0 otherwise.
int				handle_end_heredoc(char *line, char *delim);

// Writes expanded line to pipe and frees memory.
// 1. Writes expanded content.
// 2. Adds newline.
// 3. Frees both original and expanded strings.
void			write_and_free_line(int fd, char *line, char *expanded);

// Expands variables in heredoc content if enabled.
// 1. Returns direct copy if expansion disabled.
// 2. Otherwise performs variable expansion.
// Returns expanded string.
char			*get_expnd(char *line, int no_expnd, t_env *env, t_shell *sh);

// Expands variables in heredoc content.
// 1. Scans for $ variables.
// 2. Expands each variable found.
// 3. Preserves other characters literally.
// Returns expanded string.
char			*heredoc_expand_vars(char *line, t_env *env, int last_status);

// Extracts and expands variable reference from input line.
// 1. Handles special $? status variable.
// 2. Reads valid variable name characters (alphanumeric + underscore).
// 3. Looks up value in environment.
// Returns expanded value or NULL if not found.
char			*get_var_value(char *line, int *ip, t_env *env, int last_sts);

// Joins characters to string with memory management.
// 1. Allocates new string with space for additional character.
// 2. Copies original string content.
// 3. Appends new character.
// 4. Frees original string.
// Returns new combined string.
char			*ft_strjoin_char_free(char *str, char c);

#endif
