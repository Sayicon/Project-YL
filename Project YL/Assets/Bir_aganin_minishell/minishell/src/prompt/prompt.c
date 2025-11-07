/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   prompt.c                                           :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/22 18:44:54 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/23 00:48:11 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*make_prompt(t_shell *shell)
{
	char	*user;
	char	*host;
	char	*cwd;
	size_t	len;
	char	*prompt;

	user = get_user();
	host = read_hostname_file();
	cwd = get_cwd(shell);
	len = ft_strlen(user) + 1 + ft_strlen(host) + 1 + ft_strlen(cwd) + 3;
	prompt = assemble_prompt(user, host, cwd, len);
	setup_signals_prompt();
	return (prompt);
}

char	*assemble_prompt(char *user, char *host, char *cwd, size_t len)
{
	char	*prompt;
	size_t	extra;
	size_t	i;

	extra = ft_strlen(C_RED) + ft_strlen(C_RESET) + ft_strlen(C_YELLOW)
		+ ft_strlen(C_RESET);
	prompt = (char *)malloc(len + extra + 1);
	i = 0;
	build_user_host(prompt, &i, user, host);
	if (cwd)
		build_cwd(prompt, &i, cwd);
	else
		build_cwd(prompt, &i, "[deleted]");
	append_char(prompt, &i, '$');
	append_char(prompt, &i, ' ');
	append_char(prompt, &i, '\0');
	free(host);
	free(cwd);
	return (prompt);
}

// Gets curent working directory for prompt display.
// 1. Checks PWD environment variable.
// 2. Shortens path if inside home directory.
// 3. Fails back to system cwd if needed.
// Returns formatted directory string.
char	*get_cwd(t_shell *shell)
{
	char	*raw;
	char	*home;
	char	*cwd;

	raw = get_env_value(shell->env, "PWD");
	home = getenv("HOME");
	if (raw && home && ft_strncmp(raw, home, ft_strlen(home)) == 0)
		cwd = replace_home(ft_strdup(raw), home);
	else if (raw)
		cwd = ft_strdup(raw);
	else
		cwd = get_cwd_display();
	return (cwd);
}
