/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   clean.c                                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/23 11:41:48 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/19 00:36:11 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	clean_all(t_token_list *tokens, t_ast *node, t_env **env)
{
	rl_clear_history();
	if (tokens)
		token_list_free(tokens);
	if (node)
		ast_free(node);
	if (env && *env)
		env_free_all(env);
}

void	env_free_all(t_env **head)
{
	t_env	*curr;
	t_env	*next;

	curr = *head;
	while (curr)
	{
		next = curr->next;
		free(curr->key);
		free(curr->value);
		free(curr);
		curr = next;
	}
	*head = NULL;
}

void	exit_perror(const char *msg)
{
	perror(msg);
	exit(EXIT_FAILURE);
}
