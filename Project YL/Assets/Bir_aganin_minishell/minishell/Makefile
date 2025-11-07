NAME		= minishell
NAME_BONUS	= minishell_bonus
CC		= cc
CFLAGS		= -Wall -Wextra -Werror -g
RM		= rm -rf

SRC_DIR		= src
OBJ_DIR		= obj
INCLUDE		= -Iinclude
LIBS		= -lreadline
LIBFT_DIR	= libft
LIBFT_LIB	= ${LIBFT_DIR}/libft.a

CLEAN_DIR	= ${SRC_DIR}/clean
EXPANSION_DIR	= ${SRC_DIR}/expansion
TOKENIZER_DIR	= ${SRC_DIR}/tokenizer
AST_DIR		= ${SRC_DIR}/ast
SYNTAX_DIR	= ${SRC_DIR}/syntax
EXEC_DIR	= ${SRC_DIR}/executor
ENV_DIR		= ${SRC_DIR}/env
BUILTIN_DIR	= ${SRC_DIR}/builtin
PROMPT_DIR	= ${SRC_DIR}/prompt
SIGNAL_DIR	= ${SRC_DIR}/signal
HEREDOC_DIR	= ${SRC_DIR}/heredoc

SRCS		= ${SRC_DIR}/main.c \
		  ${CLEAN_DIR}/clean.c \
		  ${CLEAN_DIR}/clean_ast.c \
		  ${CLEAN_DIR}/clean_expansion.c \
		  ${TOKENIZER_DIR}/tokenizer.c \
		  ${TOKENIZER_DIR}/tokenizer_handle_operators.c \
		  ${TOKENIZER_DIR}/tokenizer_utils.c \
		  ${TOKENIZER_DIR}/token_list.c \
		  ${EXPANSION_DIR}/expansion.c \
		  ${EXPANSION_DIR}/expand_env.c \
		  ${EXPANSION_DIR}/expand_token.c \
		  ${EXPANSION_DIR}/expand_env_utils.c \
		  ${EXPANSION_DIR}/expansion_utils.c \
		  ${EXPANSION_DIR}/expand_wild.c \
		  ${EXPANSION_DIR}/expand_wild_utils.c \
		  ${EXPANSION_DIR}/expand_wild_split.c \
		  ${EXPANSION_DIR}/expand_redir.c \
		  ${EXPANSION_DIR}/expand_redir_utils.c \
		  ${EXPANSION_DIR}/handle_quotes.c \
		  ${EXPANSION_DIR}/sort_wildcard.c \
		  ${AST_DIR}/ast_utils.c \
		  ${AST_DIR}/ast.c \
		  ${AST_DIR}/commands.c \
		  ${AST_DIR}/commands_utils.c \
		  ${SYNTAX_DIR}/syntax_analysis.c \
		  ${SYNTAX_DIR}/syntax_analysis_utils.c \
		  ${SYNTAX_DIR}/syntax_errors.c \
		  ${EXEC_DIR}/executor.c \
		  ${EXEC_DIR}/executor_fds.c \
		  ${EXEC_DIR}/executor_redirs.c \
		  ${EXEC_DIR}/executor_redirs_utils.c \
		  ${EXEC_DIR}/executor_utils.c \
		  ${EXEC_DIR}/executor_status.c \
		  ${EXEC_DIR}/executor_dispatch.c \
		  ${EXEC_DIR}/executor_builtin.c \
		  ${EXEC_DIR}/executor_path_utils.c \
		  ${EXEC_DIR}/executor_command.c \
		  ${EXEC_DIR}/executor_command_external_utils.c \
		  ${EXEC_DIR}/executor_pipe.c \
		  ${ENV_DIR}/env_init.c \
		  ${ENV_DIR}/env_utils.c \
		  ${BUILTIN_DIR}/builtin_exit.c \
		  ${BUILTIN_DIR}/builtin_pwd.c \
		  ${BUILTIN_DIR}/builtin_env.c \
		  ${BUILTIN_DIR}/builtin_echo.c \
		  ${BUILTIN_DIR}/builtin_cd.c \
		  ${BUILTIN_DIR}/builtin_export.c \
		  ${BUILTIN_DIR}/builtin_export_utils.c \
		  ${BUILTIN_DIR}/builtin_unset.c \
		  ${PROMPT_DIR}/prompt.c \
		  ${PROMPT_DIR}/prompt_utils.c \
		  ${PROMPT_DIR}/prompt_colors.c \
		  ${SIGNAL_DIR}/setup_signals_prompt.c \
		  ${SIGNAL_DIR}/setup_signals_exec.c \
		  ${SIGNAL_DIR}/setup_signals_heredoc.c \
		  ${HEREDOC_DIR}/heredoc.c \
		  ${HEREDOC_DIR}/heredoc_utils.c \

OBJS		= ${SRCS:${SRC_DIR}/%.c=${OBJ_DIR}/%.o}

# Colors
RED	= \033[1;31m
GREEN	= \033[1;32m
YELLOW	= \033[1;33m
BLUE	= \033[1;34m
MAG	= \033[1;35m
CYAN	= \033[1;36m
RESET	= \033[0m

all:		${NAME}

${NAME}:	${OBJS} ${LIBFT_LIB} 
			@echo "${CYAN}[  COMPILING ]${RESET} Compiling source files..."
			@${CC} ${CFLAGS} ${OBJS} ${LIBFT_LIB} ${LIBS} -o ${NAME}
			@echo "${GREEN}✅ Build complete!${RESET}"

${OBJ_DIR}/%.o:	${SRC_DIR}/%.c
			@mkdir -p ${@D}
			@${CC} ${CFLAGS} ${INCLUDE} -I ${LIBFT_DIR} -c $< -o $@

${LIBFT_LIB}:
			@echo "${BLUE}[  BUILDING  ]${RESET} Building libft..."
			@make -C ${LIBFT_DIR} --no-print-directory

leak:
			valgrind --leak-check=full --show-leak-kinds=all --track-fds=yes --suppressions=readline.supp ./${NAME}
leakfile:
			valgrind --leak-check=full --show-leak-kinds=all --track-fds=yes --suppressions=readline.supp --log-file=valgrind.log ./${NAME}

bonus:
			@make -s re
			@cp ${NAME} ${NAME_BONUS} 

clean:
			@echo "${RED}[  CLEANING  ]${RESET} Removing object files..."
			@${RM} ${OBJ_DIR}
			@echo "${RED}[  CLEANING  ]${RESET} Removing libft object files..."
			@make -s -C ${LIBFT_DIR} clean --no-print-directory

fclean:		clean
			@echo "${RED}[  CLEANING  ]${RESET} Removing binary and libraries..."
			@${RM} ${NAME} ${NAME_BONUS}
			@make -s -C ${LIBFT_DIR} fclean --no-print-directory

re:		fclean all

.PHONY:		all clean fclean re
